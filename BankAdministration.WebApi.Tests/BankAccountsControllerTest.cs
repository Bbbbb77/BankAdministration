using System;
using Xunit;
using BankAdministration.Persistence.Models;
using BankAdministration.Persistence.Services;
using BankAdministration.WebApi.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BankAdministration.Persistence.DTOS;
using System.Linq;

namespace BankAdministration.WebApi.Tests
{
    public class BankAccountsControllerTest : IDisposable
    {
        private readonly BankAdministrationDbContext context_;
        private readonly BankAdministrationService service_;
        private readonly BankAccountsController controller_;

        public BankAccountsControllerTest()
        {
            var options = new DbContextOptionsBuilder<BankAdministrationDbContext>()
                .UseInMemoryDatabase("BankAccountsControllerTestDb2")
                .Options;

            context_ = new BankAdministrationDbContext(options);

            TestDbInitializer.Initialize(context_);

            var userManager = new UserManager<User>(
                new UserStore<User>(context_), null,
                new PasswordHasher<User>(), null, null, null, null, null, null);
            var user = new User { UserName = "testName", Id = "testId" };
            userManager.CreateAsync(user, "testPassword").Wait();

            foreach (var account in context_.BankAccounts.ToList())
            {
                account.UserId = user.Id;
                account.User = user;
            }
            context_.SaveChanges();

            service_ = new BankAdministrationService(context_);
            controller_ = new BankAccountsController(service_);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "testName"),
                new Claim(ClaimTypes.NameIdentifier, "testid")
            });
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            controller_.ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };
        }

        public void Dispose()
        {
            context_.Database.EnsureDeleted();
            context_.Dispose();
        }

        [Fact]
        public async void GetBankAccountByUserName()
        {
            var result = await controller_.GetBankAccountsByUserName("testName");
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var content = Assert.IsAssignableFrom<IEnumerable<BankAccountDto>>(objectResult.Value);
            Assert.Equal(3, content.Count());
        }

        [Fact]
        public async void SetDeposit()
        {
            Assert.Equal(1, context_.Transactions.Where(t => t.TransactionType == TransactionTypeEnum.Deposit).Count());
            var dto = new DepositDto
            {
                Number = "1234567899",
                DepositAmount = 1000
            };
            await controller_.SetDeposit(dto);
            Assert.Equal(2, context_.Transactions.Where(t => t.TransactionType == TransactionTypeEnum.Deposit).Count());
        }

        [Fact]
        public async void SetWithdrawn()
        {
            Assert.Equal(0, context_.Transactions.Where(t => t.TransactionType == TransactionTypeEnum.Withdrawn).Count());
            var dto = new WithdrawnDto
            {
                Number = "1234567899",
                WithdrawnAmount = 1000
            };
            await controller_.SetWithdrawn(dto);
            Assert.Equal(1, context_.Transactions.Where(t => t.TransactionType == TransactionTypeEnum.Withdrawn).Count());
        }

        [Fact]
        public async void SetTransfer()
        {
            Assert.Equal(1, context_.Transactions.Where(t => t.TransactionType == TransactionTypeEnum.Transfer).Count());
            var dto = new TransferDto
            {
                SourceNumber = "1234567899",
                TransferAmount = 1000,
                DestNumber = "987654321",
                DestUserName = "test2"
            };
            await controller_.SetTransfer(dto);
            Assert.Equal(2, context_.Transactions.Where(t => t.TransactionType == TransactionTypeEnum.Transfer).Count());
        }
    }
}
