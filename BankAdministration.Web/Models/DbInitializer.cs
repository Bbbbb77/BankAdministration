using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankAdministration.Web.Models
{
    public static class DbInitializer
    {
        public static void Initialize(BankAdministrationDbContext context)
        {
            context.Database.Migrate();
            if (context.BankAccounts.Any())
            {
                return;
            }

            IList<BankAccount> defaultLists = new List<BankAccount> { };

            //context.Add(defaultLists.First()); egy elem hozzáadása
            context.AddRange(defaultLists); // optimálisabb mint egyesével
            context.SaveChanges();
        }
    }
}
