using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankAdministration.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace BankAdministration.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager_;
        private readonly SignInManager<User> signInManager_;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            userManager_ = userManager;
            signInManager_ = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await signInManager_.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", "Bejelentkezés sikertelen!");
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.UserName };
                var result = await userManager_.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager_.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", "Sikertelen regisztráció");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager_.SignOutAsync();
            return RedirectToAction("Index", "BankAccounts");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            return Redirect(returnUrl);
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            /*else
            {
                return RedirectToAction(nameof(BankAccountsController.Index), "BankAccounts");
            }*/
        }
    }
}
