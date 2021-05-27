using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAdministration.Persistence.Services;
using BankAdministration.Persistence.Models;
using BankAdministration.Persistence.DTOS;

namespace BankAdministration.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IBankAdministrationService service_;

        public UsersController(IBankAdministrationService service)
        {
            service_ = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetUsersForAdmin()
        {
            try
            {
                var result = service_.GetUsersForAdmin().Select(user => (UserDto)user).ToList();
                return result;
            }catch(Exception)
            {
                return NotFound();
            }
        }
    }
}
