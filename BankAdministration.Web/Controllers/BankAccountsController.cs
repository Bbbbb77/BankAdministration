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

        public BankAccountsController(UserManager<User> userManager, SignInManager<User> signInManager, IBankAdministrationService service)
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
                    .GetBankAccounts(user));
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Balance,IsLocked,UserId")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                service_.Add(bankAccount);
                await service_.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccount);
        }*/

        // GET: BankAccounts/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccount = await service_.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Balance,IsLocked,UserId")] BankAccount bankAccount)
        {
            if (id != bankAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    service_.Update(bankAccount);
                    await service_.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankAccountExists(bankAccount.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccount = await service_.BankAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var bankAccount = await service_.BankAccounts.FindAsync(id);
            service_.BankAccounts.Remove(bankAccount);
            await service_.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankAccountExists(int id)
        {
            return service_.BankAccounts.Any(e => e.Id == id);
        }*/

        public IActionResult CreateTransaction(int id)
        {
            TempData["TransactionId"] = id;
            return RedirectToAction("Create", "Transactions");
        }
    }
}
