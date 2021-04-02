using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BankAdministration.Web.Models
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            /*BankAdministrationDbContext context = serviceProvider.GetRequiredService<BankAdministrationDbContext>();

            context.Database.Migrate();

            if (context.BankAccounts.Any())
            {
                return;
            }

            IList<BankAccount> defaultLists = new List<BankAccount>();

            defaultLists.Add
                (
                new BankAccount
                {
                    Number = "102345678901234567890123",
                    Balance = 124000,
                    IsLocked = false,
                    Transactions = new List<Transaction>()
                    {
                        new Transaction()
                        {
                            TransactionType = TransactionTypeEnum.Withdrawn,
                            SourceAccountNumber = "102345678901234567890123",
                            Amount = 2500,
                            OldBalance = 126500,
                            NewBalance = 124000,
                            TransactionTime = new DateTime(2020,1,1)
                        }
                    }
                }
                ) ;

            //context.Add(defaultLists.First()); egy elem hozzáadása
            context.AddRange(defaultLists); // optimálisabb mint egyesével
            context.SaveChanges();*/
        }
    }
}
