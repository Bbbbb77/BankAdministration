﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAdministration.Web.Models;

namespace BankAdministration.Web.Services
{
    public interface IBankAdministrationService
    {
        List<BankAccount> GetBankAccounts(User user);
        BankAccount GetBankAccountById(int id);
        bool CreateBankAccount(BankAccount bankAccount);
        bool UpdateBankAccount(BankAccount bankAccount);
        bool DeleteBankAccount(int id);
        List<Transaction> GetTransactions();
        Transaction GetTransaction(int id);
        bool CreateTransaction(Transaction transaction);
        bool UpdateTransaction(Transaction transaction);
        bool DeleteTransaction(int id);
    }
}