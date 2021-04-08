using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAdministration.Web.Models;

namespace BankAdministration.Web.Services
{
    public interface IBankAdministrationService
    {
        User GetUserById(string id);
        List<User> GetUsers();
        Task<List<BankAccount>> GetBankAccountsByUser(User user);
        List<BankAccount> GetBankAccounts();
        BankAccount GetBankAccountById(int id);
        bool CreateBankAccount(BankAccount bankAccount);
        bool UpdateBankAccount(BankAccount bankAccount);
        bool DeleteBankAccount(int id);
        List<Transaction> GetTransactions();
        Transaction GetTransaction(int id);
        bool CreateTransaction(Transaction transaction);
        bool UpdateTransaction(Transaction transaction);
        bool DeleteTransaction(int id);
        bool CheckBankAccount(string newBankAccountNumber);
        void TransferBetweenAccounts(string sourceAccount, string destAccount, Int64 amount);
        BankAccount GetBankAccountByNumber(string number);

        string? GetUserIdByName(string username);
    }
}
