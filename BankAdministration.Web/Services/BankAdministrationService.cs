using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAdministration.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAdministration.Web.Services
{
    public class BankAdministrationService : IBankAdministrationService
    {
        private readonly BankAdministrationDbContext _context;

        public BankAdministrationService(BankAdministrationDbContext context)
        {
            _context = context;
        }

        public List<BankAccount> GetBankAccounts(User user)
        {
            return _context.BankAccounts
                .Where(l => l.User == user)
                .OrderBy(l => l.Id)
                .ToList();
        }

        public BankAccount GetBankAccountById(int id)
        {
            return _context.BankAccounts
                .Include(l => l.Transactions)
                .Single(l => l.Id == id); // throws exception if id not found
        }

        public bool CreateBankAccount(BankAccount bankAccount)
        {
            try
            {
                _context.Add(bankAccount);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool UpdateBankAccount(BankAccount bankAccount)
        {
            try
            {
                _context.Update(bankAccount);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool DeleteBankAccount(int id)
        {
            var bankAccount = _context.BankAccounts.Find(id);
            if (bankAccount == null)
                return false;

            try
            {
                _context.Remove(bankAccount);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public List<Transaction> GetTransactions()
        {
            return _context.Transactions
                .OrderBy(i => i.TransactionTime)
                .ToList();
        }

        public Transaction GetTransaction(int id)
        {
            return _context.Transactions
                //.Include(i => i.BankAccounts)
                .FirstOrDefault(i => i.Id == id);
        }

        public bool CreateTransaction(Transaction transaction)
        {
            try
            {
                _context.Add(transaction);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool UpdateTransaction(Transaction transaction)
        {
            try
            {
                _context.Update(transaction);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool DeleteTransaction(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
                return false;

            try
            {
                _context.Remove(transaction);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

    }
}
