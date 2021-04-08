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
        public async Task<IActionResult> Index()
        {
            User user = await CurrentUser();
            return View(await service_
                    .GetBankAccountsByUser(user));
        }

        // GET: BankAccounts/Details/5
       public async Task<IActionResult> Details(int id)
        {
            var bankAccount = service_.GetBankAccountById(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        public IActionResult Create()
        {
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
        public async Task<IActionResult> Create([Bind("Id,Number,Balance,IsLocked,UserId")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                bool result = service_.CreateBankAccount(bankAccount);
                if(result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Create BankAccount is unsuccessful!");
                    //return NotFound();
                }

            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            //TDOD transfer reaminig balance
            var bankAccount = service_.GetBankAccountById(id);
            
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
            var item = service_.GetBankAccountById(id);
            if (item != null)
            {
                service_.DeleteBankAccount(id);
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        public IActionResult CreateTransaction(int id)
        {
            TempData["BankAccountId"] = id;
            return RedirectToAction("Create", "Transactions");
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
                var result = await signInManager_.PasswordSignInAsync(model.UserName,
                                                                      model.Password,
                                                                      isPersistent: false,
                                                                      lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(BankAccountsController.Index), "BankAccounts");
                }
            }
            return View(model);
        }
    }
}
