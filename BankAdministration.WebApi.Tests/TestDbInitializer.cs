using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankAdministration.Persistence.Models;

namespace BankAdministration.WebApi.Tests
{
    public class TestDbInitializer
    {
        public static void Initialize(BankAdministrationDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.BankAccounts.Any())
            {
                return;
            }

            List<BankAccount> bankAccounts1 = new List<BankAccount>();
            var bankAccount1 = new BankAccount
            {
                Number = "1234567899",
                Balance = 1000,
                IsLocked = false,
                CreatedDate = DateTime.Parse("2/2/2021"),
                Transactions = new List<Transaction>(),
                User = null
            };
            var bankAccount2 = new BankAccount
            {
                Number = "1234569999",
                Balance = 5555,
                IsLocked = true,
                CreatedDate = DateTime.Parse("1/1/2021"),
                Transactions = new List<Transaction>(),
                User = null
            };
            bankAccounts1.Add(bankAccount1);
            bankAccounts1.Add(bankAccount2);

            List<Transaction> transactions1 = new List<Transaction>();
            Transaction transaction1 = new Transaction
            {
                TransactionType = TransactionTypeEnum.Deposit,
                SourceAccountNumber = "1234567899",
                DestinationAccountNumber = "1234567899",
                DestinationAccountUserName = "Anna",
                Amount = 7000,
                OldBalance = 0,
                NewBalance = 7000,
                TransactionTime = DateTime.Parse("2/1/2021"),
                BankAccount = bankAccount1
            };

            Transaction transaction2 = new Transaction
            {
                TransactionType = TransactionTypeEnum.Transfer,
                SourceAccountNumber = "1234567899",
                DestinationAccountNumber = "1232343234",
                DestinationAccountUserName = "Józsi",
                Amount = 6000,
                OldBalance = 7000,
                NewBalance = 1000,
                TransactionTime = DateTime.Parse("5/4/2021"),
                BankAccount = bankAccount1
            };

            transactions1.Add(transaction1);
            transactions1.Add(transaction2);
            bankAccount1.Transactions = transactions1;



            List<BankAccount> bankAccounts2 = new List<BankAccount>();
            var bankAccount3 = new BankAccount
            {
                Number = "9988073748",
                Balance = 1000,
                IsLocked = false,
                CreatedDate = DateTime.Parse("3/3/2021"),
                Transactions = new List<Transaction>(),
                User = null
            };
            bankAccounts2.Add(bankAccount3);

            context.BankAccounts.Add(bankAccount1);
            context.BankAccounts.Add(bankAccount2);
            context.BankAccounts.Add(bankAccount3);

            context.Transactions.Add(transaction1);
            context.Transactions.Add(transaction2);

            context.SaveChanges();
        }
    }
}
