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
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace BankAdministration.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly IBankAdministrationService service_;

        public BankAccountsController(IBankAdministrationService service)
        {
            service_ = service;
        }

        // GET: api/BankAccounts
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<BankAccountDto>> GetBankAccounts()
        {
            var result = service_.GetBankAccounts().Select(bankAccounts => (BankAccountDto)bankAccounts).ToList();
            return result;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBankAccountsByUserName(string userName)
        {
            try
            {
                var result = Ok((await service_.GetBankAccountsByUserName(userName))
                    .Select(bankAccount => (BankAccountDto)bankAccount));
                return result;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetLock(LockDto dto)
        {
            try
            {
                var result = Ok((service_.SetLocking(dto.IsLocked, dto.Number)));
                return result;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetDeposit(DepositDto dto)
        {
            try
            {
                var result = Ok((service_.SetDeposit(dto.DepositAmount, dto.Number)));
                return result;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetTransfer(TransferDto dto)
        {
            try
            {
                var result = Ok((service_.SetTransfer(dto.TransferAmount, dto.SourceNumber,
                                                        dto.DestNumber, dto.DestUserName)));
                return result;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetWithdrawn(WithdrawnDto dto)
        {
            try
            {
                var result = Ok((service_.SetWithdrawn(dto.WithdrawnAmount, dto.Number)));
                return result;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
