using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAdministration.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAdministration.Persistence.Services
{
    public class BankAdministrationService : IBankAdministrationService
    {
        private readonly BankAdministrationDbContext context_;

        public BankAdministrationService(BankAdministrationDbContext context)
        {
            context_ = context;
        }

        public List<User> GetUsers()
        {
            return context_.Users.Include(l => l.BankAccounts).ToList();
        }

        public async Task<List<BankAccount>> GetBankAccountsByUser(User user)
        {
            return context_.BankAccounts
                .Where(l => l.User == user)
                .OrderBy(l => l.Id)
                .ToList();
        }

        public async Task<List<BankAccount>> GetBankAccountsByUserName(string userName)
        {
            return context_.BankAccounts.Where(bankAccount => bankAccount.User.UserName == userName).ToList();
        }

        public List<BankAccount> GetBankAccounts()
        {
            return context_.BankAccounts.ToList();
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

        public void TransferBetweenAccounts(string sourceAccount, string sestAccount, Int64 amount)
        {
            var sAccount = context_.BankAccounts.SingleOrDefault(i => i.Number == sourceAccount);
            sAccount.Balance -= amount; 

            var dAccount = context_.BankAccounts.SingleOrDefault(i => i.Number == sestAccount);
            if(dAccount != null)
            {
                dAccount.Balance += amount;
            }
            context_.SaveChanges();
        }

        public BankAccount GetBankAccountByNumber(string number)
        {
            return context_.BankAccounts.Include(l => l.User).SingleOrDefault(i => i.Number == number);
        }

        public string? GetUserIdByName(string username)
        {
            var user = context_.Users.SingleOrDefault(u => u.UserName == username);
            if(user == null)
            {
                return null;
            }
            else
            {
                return user.Id;
            } 
        }

        public BankAccount GetBankAccountByUserAndId(User user, int id)
        {
            return context_.BankAccounts.Include(l => l.User)
                                        .Include(l => l.Transactions)
                                        .SingleOrDefault(i => i.Id == id && i.User == user);
        }

        public async Task<List<Transaction>> GetTransactionsByAccountNumber(string bankAccountNumber)
        {
            var result = context_.Transactions
                .Where(trans => trans.BankAccount.Number == bankAccountNumber)
                .ToList();
            return result;
        }
    }
}
