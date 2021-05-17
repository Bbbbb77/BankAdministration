﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankAdministration.Persistence.Models;
using BankAdministration.Persistence.Services;
using BankAdministration.Persistence.DTOS;

namespace BankAdministration.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly IBankAdministrationService service_;

        public BankAccountsController(IBankAdministrationService service)
        {
            service_ = service;
        }

        // GET: api/BankAccounts
        [HttpGet]
        public ActionResult<IEnumerable<BankAccountDto>> GetBankAccounts()
        {
            return service_.GetBankAccounts().Select(bankAccounts => (BankAccountDto)bankAccounts).ToList();
        }

        /*
        // GET: api/BankAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccount>> GetBankAccount(int id)
        {
            var bankAccount = await service_.BankAccounts.FindAsync(id);

            if (bankAccount == null)
            {
                return NotFound();
            }

            return bankAccount;
        }

        // PUT: api/BankAccounts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankAccount(int id, BankAccount bankAccount)
        {
            if (id != bankAccount.Id)
            {
                return BadRequest();
            }

            service_.Entry(bankAccount).State = EntityState.Modified;

            try
            {
                await service_.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankAccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BankAccounts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankAccount)
        {
            service_.BankAccounts.Add(bankAccount);
            await service_.SaveChangesAsync();

            return CreatedAtAction("GetBankAccount", new { id = bankAccount.Id }, bankAccount);
        }

        // DELETE: api/BankAccounts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BankAccount>> DeleteBankAccount(int id)
        {
            var bankAccount = await service_.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            service_.BankAccounts.Remove(bankAccount);
            await service_.SaveChangesAsync();

            return bankAccount;
        }

        private bool BankAccountExists(int id)
        {
            return service_.BankAccounts.Any(e => e.Id == id);
        }
        */
    }
}
