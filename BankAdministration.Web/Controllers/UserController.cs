using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankAdministration.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using BankAdministration.Web.Services;
using Microsoft.AspNetCore.Http;

namespace BankAdministration.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager_;
        private readonly SignInManager<User> signInManager_;
        private readonly IBankAdministrationService service_;

        public UserController(UserManager<User> userManager, 
                              SignInManager<User> signInManager, 
                              IBankAdministrationService service)
        {
            userManager_ = userManager;
            signInManager_ = signInManager;
            service_ = service;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = service_.GetUserIdByName(model.UserName);
                if(userId == null)
                {
                    ModelState.AddModelError("", "User not exists!");
                    return View("Login", model);
                }

                bool pincodeCheck = service_ 
                    .GetUsers()
                    .Where(u => u.Pincode == model.Pincode &&
                                u.UserName == model.UserName)
                    .Any();

                if (!pincodeCheck)
                {
                    ModelState.AddModelError("", "Pincode is invalid");
                    return View("Login", model);
                }

                bool BankAccountCheck = service_
                                .GetBankAccounts()
                                .Where(u => u.UserId == userId && u.Number == model.BankAccount)
                                .Any();
                //BankAccountCheck = service_;;

                if (!BankAccountCheck)
                {
                    ModelState.AddModelError("", "Bankaccount number is invalid");
                    return View("Login", model);
                }

                var result = await signInManager_.PasswordSignInAsync(model.UserName,
                                                      model.Password,
                                                      isPersistent: false,
                                                      lockoutOnFailure: false);

                string safeMode = model.IsSafeMode ? "true" : "false";
                HttpContext.Session.SetString("SafeMode", safeMode);
                if (safeMode == "true")
                {
                    HttpContext.Session.SetString("UserIsAuthorized", "false");
                }

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(BankAccountsController.Index), "BankAccounts");
                }          

                ModelState.AddModelError("", "Unsuccesful log in!");
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var newBankAccount = new StringBuilder(10);
            var random = new Random();
            
            do {
                newBankAccount.Append(random.Next(1, 9).ToString());
                for (int i = 0; i < 9; i++)
                    newBankAccount.Append(random.Next(0, 9).ToString());
            } while (!service_.CheckBankAccount(newBankAccount.ToString()));

            ViewData["NewBankAccount"] = newBankAccount.ToString();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bankAccount = new BankAccount
                {
                    Number = model.BankAccount,
                    Balance = 0,
                    IsLocked = false,
                    CreatedDate = DateTime.Today
                };

                var user = new User
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Pincode = model.Pincode,
                    BankAccounts = new List<BankAccount> { bankAccount }
                };

                var result = await userManager_.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager_.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(BankAccountsController.Index), "BankAccounts");
                }

                ModelState.AddModelError("", "Unsuccesful registration!");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager_.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
