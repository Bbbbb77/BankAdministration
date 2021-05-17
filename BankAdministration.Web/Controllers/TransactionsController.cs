using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankAdministration.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BankAdministration.Persistence.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using BankAdministration.Persistence.Models;

namespace BankAdministration.Web.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly UserManager<User> userManager_;
        private readonly IBankAdministrationService service_;

        public TransactionsController(UserManager<User> userManager, IBankAdministrationService service)
        {
            userManager_ = userManager;
            service_ = service;
        }

        private async Task<User> CurrentUser()
        {
            ClaimsPrincipal currentUser = this.User;
            string currentUserId = currentUser
                .FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = await userManager_.FindByIdAsync(currentUserId);

            return user;
        }

        // GET: Transactions/Create
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            if (HttpContext.Session.GetString("SafeMode") == "true")
            {
                if (HttpContext.Session.GetString("UserIsAuthorized") == "false")
                {
                    HttpContext.Session.SetInt32("DetailsId", id);
                    HttpContext.Session.SetString("SafeModeAction", "TransactionCreate");
                    return RedirectToAction("SafeMode", "BankAccounts");
                }
            }

            BankAccount acc;
            User user = await CurrentUser();
            if (HttpContext.Session.GetInt32("DetailsId") == null)
            {
                acc = service_.GetBankAccountByUserAndId(user, id);
            }
            else
            {
                
                acc = service_.GetBankAccountByUserAndId(user, (int)HttpContext.Session.GetInt32("DetailsId"));
            }

            if (acc == null)
            {
                return NotFound();
            }

            if (acc.IsLocked)
            {
                return NotFound();
            }

            ViewData["SourceAccountNumber"] = acc.Number;
            ViewData["SourceAccountId"] = acc.Id;
            return View();
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTransactionViewModel transactionModel)
        {
            var SourceAccount = service_.GetBankAccountByNumber(transactionModel.SourceAccountNumber);
            ViewData["SourceAccountId"] = SourceAccount.Id;
            ViewData["SourceAccountNumber"] = transactionModel.SourceAccountNumber;
            if (ModelState.IsValid)
            {
                if(SourceAccount != null && 
                   SourceAccount.Balance < transactionModel.Amount && 
                   transactionModel.TransactionType != TransactionTypeEnum.Deposit)
                {
                    ModelState.AddModelError("", "Transaction amount cannot be bigger the balance!");
                    return View(transactionModel);
                }

                Int64 oldBalance = SourceAccount.Balance;
                Int64 newBalance;
                if(transactionModel.TransactionType == TransactionTypeEnum.Deposit)
                {
                    newBalance = SourceAccount.Balance + transactionModel.Amount;
                    SourceAccount.Balance += transactionModel.Amount;
                }
                else
                {
                    newBalance = SourceAccount.Balance - transactionModel.Amount;
                    SourceAccount.Balance -= transactionModel.Amount;
                }
                service_.UpdateBankAccount(SourceAccount);

                Transaction tranasaction = new Transaction
                {
                    TransactionType = transactionModel.TransactionType,
                    SourceAccountNumber = transactionModel.SourceAccountNumber,
                    DestinationAccountNumber = transactionModel.DestinationAccountNumber,
                    DestinationAccountUserName = transactionModel.DestinationAccountUserName,
                    Amount = transactionModel.Amount,
                    OldBalance = oldBalance,
                    NewBalance = newBalance,
                    TransactionTime = DateTime.Now.Date,
                    BankAccountId = SourceAccount.Id
                };

                Transaction transferTranasaction = null;
                bool transferResult = false;
                if (transactionModel.TransactionType == TransactionTypeEnum.Transfer)
                {
                    BankAccount destAccount = service_.GetBankAccountByNumber(transactionModel.DestinationAccountNumber);
                    if(destAccount != null)
                    {
                        if (destAccount.User.UserName == transactionModel.DestinationAccountUserName)
                        {
                            Int64 destOldBalance = destAccount.Balance;
                            Int64 destNewBalance = destAccount.Balance + transactionModel.Amount;
                            service_.UpdateBankAccount(destAccount);
                            transferTranasaction = new Transaction
                            {
                                TransactionType = TransactionTypeEnum.Deposit,
                                SourceAccountNumber = transactionModel.SourceAccountNumber,
                                DestinationAccountNumber = transactionModel.DestinationAccountNumber,
                                DestinationAccountUserName = destAccount.User.UserName,
                                Amount = transactionModel.Amount,
                                OldBalance = destOldBalance,
                                NewBalance = destNewBalance,
                                TransactionTime = DateTime.Now.Date,
                                BankAccountId = destAccount.Id
                            };

                            transferResult = service_.CreateTransaction(transferTranasaction);
                            if (!transferResult)
                            {
                                ModelState.AddModelError("", "Could not create transfer between bankaccounts!");
                                return View(transactionModel);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Username not match with the bankaccount!");
                            return View(transactionModel);
                        }
                    }
                    service_.TransferBetweenAccounts(
                    transactionModel.SourceAccountNumber,
                    transactionModel.DestinationAccountNumber,
                    transactionModel.Amount);

                }
                

                bool result = service_.CreateTransaction(tranasaction);
                
                if(result)
                {
                    HttpContext.Session.SetString("UserIsAuthorized", "false");
                    return RedirectToAction("Details",
                        new RouteValueDictionary(
                            new { Controller = "BankAccounts", Action = "Details", Id = SourceAccount.Id }));
                }
                else
                {
                    if (transferResult && transferTranasaction != null)
                    {
                        service_.DeleteTransaction(transferTranasaction.Id);
                    }
                    ModelState.AddModelError("", "Could not create transfer!");
                    return View(transactionModel);
                }
                
            }
            return View(transactionModel);
        }
    }
}
