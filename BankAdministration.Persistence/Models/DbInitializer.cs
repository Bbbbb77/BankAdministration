using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace BankAdministration.Persistence.Models
{
    public class DbInitializer
    {
        private static BankAdministrationDbContext context_;
        private static UserManager<User> userManager_;
        private static RoleManager<IdentityRole<string>> roleManager_;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            context_ = serviceProvider.GetRequiredService<BankAdministrationDbContext>();
            userManager_ = serviceProvider.GetRequiredService<UserManager<User>>();
            roleManager_ = serviceProvider.GetRequiredService<RoleManager<IdentityRole<string>>>();

            context_.Database.EnsureDeleted();
            context_.Database.Migrate();

            #region TestUser1

            User testUser1 = new User
            {
                UserName = "Anna",
                FullName = "Nagy Anna",
                Pincode = 123456,
                BankAccounts = new List<BankAccount>()
            };

            List<BankAccount> bankAccounts1 = new List<BankAccount>();
            var bankAccount1 = new BankAccount
            {
                Number = "1234567899",
                Balance = 1000,
                IsLocked = false,
                CreatedDate = DateTime.Parse("2/2/2021"),
                Transactions = new List<Transaction>(),
                User = testUser1
            };
            var bankAccount2 = new BankAccount
            {
                Number = "1234569999",
                Balance = 5555,
                IsLocked = true,
                CreatedDate = DateTime.Parse("1/1/2021"),
                Transactions = new List<Transaction>(),
                User = testUser1
            };
            bankAccounts1.Add(bankAccount1);
            bankAccounts1.Add(bankAccount2);

            List<Transaction> transactions1 = new List<Transaction>();
            transactions1.Add(
                new Transaction
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
                }
            );
            transactions1.Add(
                new Transaction
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
                }
            );

            bankAccount1.Transactions = transactions1;
            testUser1.BankAccounts = bankAccounts1;
            //testUser1.Id = Guid.NewGuid().ToString();
            var res1 = userManager_.CreateAsync(testUser1, "Alma123").Result;

            #endregion


            #region TestUser2

            User testUser2 = new User
            {
                UserName = "Bela",
                FullName = "Kovács Béla",
                Pincode = 654321,
                BankAccounts = new List<BankAccount>()
            };

            List<BankAccount> bankAccounts2 = new List<BankAccount>();
            var bankAccount11 = new BankAccount
            {
                Number = "9988073748",
                Balance = 1000,
                IsLocked = false,
                CreatedDate = DateTime.Parse("3/3/2021"),
                Transactions = new List<Transaction>(),
                User = testUser2
            };
            bankAccounts2.Add(bankAccount11);

            testUser2.BankAccounts = bankAccounts2;
            //testUser2.Id = Guid.NewGuid().ToString();
            var res2 = userManager_.CreateAsync(testUser2, "Banana123").Result;

            #endregion

            #region Admin
            User admin = new User
            {
                UserName = "admin",
                FullName = "Administrator",
                Pincode = 111111,
                BankAccounts = new List<BankAccount>()
            };

            List<BankAccount> adminBankAccounts = new List<BankAccount>();
            var adminBankAccount = new BankAccount
            {
                Number = "1111111111",
                Balance = 0,
                IsLocked = false,
                CreatedDate = DateTime.Parse("3/3/2021"),
                Transactions = new List<Transaction>(),
                User = admin
            };
            adminBankAccounts.Add(adminBankAccount);
            admin.BankAccounts = adminBankAccounts;
            //admin.Id = Guid.NewGuid().ToString();
            var adminRole = new IdentityRole<string>("administrator");
            adminRole.Id = Guid.NewGuid().ToString();
            var res3 = userManager_.CreateAsync(admin, "Admin123").Result;
            var res4 = roleManager_.CreateAsync(adminRole).Result;
            var res5 = userManager_.AddToRoleAsync(admin, adminRole.Name).Result;
            #endregion

            context_.SaveChanges();
        }
    }
}
