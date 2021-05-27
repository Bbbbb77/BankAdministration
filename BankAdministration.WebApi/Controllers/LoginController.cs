using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAdministration.Persistence.Models;
using Microsoft.AspNetCore.Authorization;
using BankAdministration.Persistence.DTOS;

namespace BankAdministration.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<User> signInManager_;
        private readonly UserManager<User> userManager_;

        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            signInManager_ = signInManager;
            userManager_ = userManager;
        }

        // api/Account/Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            if (signInManager_.IsSignedIn(User))
                await signInManager_.SignOutAsync();

            var result = await signInManager_.PasswordSignInAsync(user.UserName, 
                                                                  user.Password, 
                                                                  isPersistent: false,
                                                                  lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var admins = await userManager_.GetUsersInRoleAsync("administrator");
                var currentUser = await userManager_.FindByNameAsync(user.UserName);
                if (!admins.Contains(currentUser))
                    return Unauthorized("Login failed! User is not administrator!");

                return Ok();
            }

            return Unauthorized("Login failed!");
        }

        // api/Account/Logout
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager_.SignOutAsync();

            return Ok();
        }
    }
}
