using BankAdministration.Persistence.DTOS;
using BankAdministration.Persistence.Models;
using BankAdministration.Persistence.Services;
using BankAdministration.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BankAdministration.WebApi.Tests
{
    public class TransactionsControllerTest : IDisposable
    {
        private readonly BankAdministrationDbContext context_;
        private readonly BankAdministrationService service_;
        private readonly TransactionsController controller_;

        public TransactionsControllerTest()
        {
            var options = new DbContextOptionsBuilder<BankAdministrationDbContext>()
                .UseInMemoryDatabase("BankAccountsControllerTestDb")
                .Options;

            context_ = new BankAdministrationDbContext(options);

            TestDbInitializer.Initialize(context_);

            service_ = new BankAdministrationService(context_);
            controller_ = new TransactionsController(service_);
        }

        public void Dispose()
        {
            context_.Database.EnsureDeleted();
            context_.Dispose();
        }

        [Fact]
        public async void GetTransactions()
        {
            var result = await controller_.GetTransactions("1234567899");
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var content = Assert.IsAssignableFrom<IEnumerable<TransactionDto>>(objectResult.Value);
            Assert.Equal(2, content.Count());
        }
    }
}
