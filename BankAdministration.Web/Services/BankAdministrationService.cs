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
        private readonly BankAdministrationDbContext context_;

        public BankAdministrationService(BankAdministrationDbContext context)
        {
            context_ = context;
        }

        public User GetUserById(string id)
        {
            return context_.Users
                .Include(l => l.BankAccounts)
                .FirstOrDefault(l => l.Id == id);
        }

        public List<User> GetUsers()
        {
            return context_.Users.ToList();
        }

        public async Task<List<BankAccount>> GetBankAccounts(User user)
        {
            return context_.BankAccounts
                .Where(l => l.User == user)
                .OrderBy(l => l.Id)
                .ToList();
        }

        public List<BankAccount> AllBankAccounts()
        {
            return context_.BankAccounts
                .OrderBy(l => l.Id)
                .ToList();
        }

        public BankAccount GetBankAccountById(int id)
        {
            return context_.BankAccounts
                .Include(l => l.Transactions)
                .FirstOrDefault(l => l.Id == id);
        }

        public bool CreateBankAccount(BankAccount bankAccount)
        {
            try
            {
                context_.Add(bankAccount);
                context_.SaveChanges();
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
                context_.Update(bankAccount);
                context_.SaveChanges();
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
            var bankAccount = context_.BankAccounts.Find(id);
            if (bankAccount == null)
                return false;

            try
            {
                context_.Remove(bankAccount);
                context_.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public List<Transaction> GetTransactions()
        {
            return context_.Transactions
                .OrderBy(i => i.TransactionTime)
                .ToList();
        }

        public Transaction GetTransaction(int id)
        {
            return context_.Transactions
                .Include(i => i.BankAccount)
                .FirstOrDefault(i => i.Id == id);
        }

        public bool CreateTransaction(Transaction transaction)
        {
            try
            {
                context_.Add(transaction);
                context_.SaveChanges();
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
                context_.Update(transaction);
                context_.SaveChanges();
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
            var transaction = context_.Transactions.Find(id);
            if (transaction == null)
                return false;

            try
            {
                context_.Remove(transaction);
                context_.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool CheckBankAccount(string newBankAccountNumber)
        {
            return context_.BankAccounts.SingleOrDefault(i => i.Number == newBankAccountNumber) == null;
        }
    }
}
