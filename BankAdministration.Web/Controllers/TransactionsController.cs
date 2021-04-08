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
using BankAdministration.Web.Services;

namespace BankAdministration.Web.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly IBankAdministrationService service_;

        public TransactionsController(IBankAdministrationService service)
        {
            service_ = service;
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TransactionType,SourceAccountNumber,DestinationAccountNumber,Amount,OldBalance,NewBalance,TransactionTime,BankAccountId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var SourceAccount = service_.GetBankAccountByNumber(transaction.SourceAccountNumber);
                if(SourceAccount != null && SourceAccount.Balance < transaction.Amount)
                {
                    return View(transaction); //TODO Vagy valami hasonló
                }

                service_.TransferBetweenAccounts(transaction.SourceAccountNumber, transaction.DestinationAccountNumber, transaction.Amount);

                bool result = service_.CreateTransaction(transaction);
                
                if(result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
                
            }
            return View(transaction);
        }

        public async Task<IActionResult> BackToDetails(int bankAccountId)
        {
            return RedirectToAction(nameof(BankAccountsController.Details), "BankAccounts", bankAccountId);
        }
    }
}
