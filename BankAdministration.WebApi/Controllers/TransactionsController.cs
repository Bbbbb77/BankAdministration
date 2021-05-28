using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankAdministration.Persistence.Models;
using BankAdministration.Persistence.Services;
using BankAdministration.Persistence.DTOS;
using Microsoft.AspNetCore.Authorization;

namespace BankAdministration.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IBankAdministrationService service_;

        public TransactionsController(IBankAdministrationService service)
        {
            service_ = service;
        }

        // GET: api/Transactions
        [HttpGet]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> GetTransactions(string bankAccountNumber)
        {
            try
            {
                return Ok((await service_.GetTransactionsByAccountNumber(bankAccountNumber))
                    .Select(transaction => (TransactionDto)transaction));
            }
            catch(Exception)
            {
                return NotFound();
            }
        }
    }
}
