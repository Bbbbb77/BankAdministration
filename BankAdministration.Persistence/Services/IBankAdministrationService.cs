using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAdministration.Persistence.Models;

namespace BankAdministration.Persistence.Services
{
    public interface IBankAdministrationService
    {
        List<User> GetUsers();
        Task<List<BankAccount>> GetBankAccountsByUser(User user);
        Task<List<BankAccount>> GetBankAccountsByUserName(string userName);
        List<BankAccount> GetBankAccounts();
        BankAccount GetBankAccountById(int id);
        bool CreateBankAccount(BankAccount bankAccount);
        bool UpdateBankAccount(BankAccount bankAccount);
        bool DeleteBankAccount(int id);
        List<Transaction> GetTransactions();
        Transaction GetTransaction(int id);
        bool CreateTransaction(Transaction transaction);
        bool DeleteTransaction(int id);
        bool CheckBankAccount(string newBankAccountNumber);
        void TransferBetweenAccounts(string sourceAccount, string destAccount, Int64 amount);
        BankAccount GetBankAccountByNumber(string number);
        string? GetUserIdByName(string username);
        BankAccount GetBankAccountByUserAndId(User user, int id);
        Task<List<Transaction>> GetTransactionsByAccountNumber(string bankAccountNumber);
    }
}
