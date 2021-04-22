using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankAdministration.Web.Models;
using Microsoft.AspNetCore.Authorization;
using BankAdministration.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BankAdministration.Web.Controllers
{
    [Authorize]
    public class BankAccountsController : Controller
    {
        private readonly UserManager<User> userManager_;
        private readonly SignInManager<User> signInManager_;
        private readonly IBankAdministrationService service_;

        public BankAccountsController(UserManager<User> userManager, 
                                      SignInManager<User> signInManager, 
                                      IBankAdministrationService service)
        {
            userManager_ = userManager;
            signInManager_ = signInManager;
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

        // GET: BankAccounts
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("UserIsAuthorized", "false");
            User user = await CurrentUser();
            return View(await service_
                    .GetBankAccountsByUser(user));
        }

        // GET: BankAccounts/Details/5
       [HttpGet]
       public async Task<IActionResult> Details(int id)
        {
            HttpContext.Session.SetString("UserIsAuthorized", "false");
            var bankAccount = service_.GetBankAccountById(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        [HttpGet]
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("SafeMode") == "true")
            {
                if(HttpContext.Session.GetString("UserIsAuthorized") == "false")
                {
                    HttpContext.Session.SetString("SafeModeAction", "BankAccountsCreate");
                    return RedirectToAction(nameof(SafeMode),"BankAccounts");
                }
            }

            var newBankAccount = new StringBuilder(10);
            var random = new Random();

            do
            {
                newBankAccount.Append(random.Next(1, 9).ToString());
                for (int i = 0; i < 9; i++)
                    newBankAccount.Append(random.Next(0, 9).ToString());
            } while (!service_.CheckBankAccount(newBankAccount.ToString()));

            ViewData["CreateNewBankAccount"] = newBankAccount.ToString();
            return View();
        }

        // POST: BankAccounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBankAccountViewModel bankAccountModel)
        {
            if (ModelState.IsValid)
            {
                User user = await CurrentUser();
                BankAccount bankAccount = new BankAccount
                {
                    Number = bankAccountModel.Number,
                    Balance = bankAccountModel.Balance,
                    IsLocked = bankAccountModel.IsLocked,
                    CreatedDate = DateTime.Now.Date,
                    User = user
                };

                bool result = service_.CreateBankAccount(bankAccount);
                if(result)
                {
                    HttpContext.Session.SetString("UserIsAuthorized", "false");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Create BankAccount is unsuccessful!");
                }

            }
            return View(bankAccountModel);
        }

        // GET: BankAccounts/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["DetailsId"] = id;
            
            if (HttpContext.Session.GetString("SafeMode") == "true")
            {
                if (HttpContext.Session.GetString("UserIsAuthorized") == "false")
                {
                    HttpContext.Session.SetInt32("DetailsId", id);
                    HttpContext.Session.SetString("SafeModeAction", "BankAccountsDelete");
                    return RedirectToAction(nameof(SafeMode), "BankAccounts");
                }
            }

            //TDOD transfer reaminig balance
            //var bankAccount = service_.GetBankAccountById(Int32.Parse(ViewData["DetailsId"].ToString()));
            //var dsds = HttpContext.Session.GetInt32("DetailsId");
            var bankAccount = service_.GetBankAccountById( (int)HttpContext.Session.GetInt32("DetailsId"));

            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bankAccount = service_.GetBankAccountById(id);
            if (bankAccount != null)
            {
                bool result = service_.DeleteBankAccount(id);
                if (result)
                {
                    HttpContext.Session.SetString("UserIsAuthorized", "false");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Could not delete BankAccount!");
                }
            }

            return View(bankAccount);
        }

        public IActionResult CreateTransaction(int id)
        {
            return RedirectToAction("Create", new RouteValueDictionary(new { Controller = "Transactions", Action = "Create", Id = id }));
        }


        [HttpGet]
        public async Task<IActionResult> SafeMode()
        {
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> SafeMode(SafeModeViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Password == null)
                {
                    ModelState.AddModelError("", "Password is necessary to proceed further!");
                    return View(model);
                }

                if (model.UserName == null)
                {
                    ModelState.AddModelError("", "UserName is necessary to proceed further!");
                    return View(model);
                }

                User user = await CurrentUser();

                bool userNameCheck = user.UserName == model.UserName;
               
                var result = userManager_
                    .PasswordHasher
                    .VerifyHashedPassword(user, user.PasswordHash, model.Password);

                if (result != PasswordVerificationResult.Failed && userNameCheck)
                {
                    HttpContext.Session.SetString("UserIsAuthorized", "true");
                    switch (HttpContext.Session.GetString("SafeModeAction"))
                    {
                        case "BankAccountsCreate": return RedirectToAction(nameof(BankAccountsController.Create), "BankAccounts");
                        case "BankAccountsDelete": return RedirectToAction(nameof(BankAccountsController.Delete), "BankAccounts");
                        case "TransactionCreate" : return RedirectToAction(nameof(TransactionsController.Create), "Transactions");
                        default: return RedirectToAction(nameof(BankAccountsController.Index), "BankAccounts");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is not correct!");
                }
            }
            return View(model);
        }

        public IActionResult Back()
        {
            
            switch (HttpContext.Session.GetString("SafeModeAction"))
            {
                case "BankAccountsCreate": return RedirectToAction(nameof(BankAccountsController.Index), "BankAccounts");
                case "BankAccountsDelete": return RedirectToAction(nameof(BankAccountsController.Index), "BankAccounts");
                case "TransactionCreate":
                    int bankAccountId = (int)HttpContext.Session.GetInt32("DetailsId");
                    return RedirectToAction("Details", new RouteValueDictionary(
                                            new { Controller = "BankAccounts", Action = "Details", Id = bankAccountId }));
                default: return RedirectToAction(nameof(BankAccountsController.Index), "BankAccounts");
            }
        }
    }
}
