using Login.Demo.Web.Entities;
using Login.Demo.Web.Repository;
using Login.Demo.Web.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Demo.Web.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class UserController : ControllerBase
    {
        RegisterAccount registerAccount;
        UserService userService;
        public UserController(IConfiguration configuration)
        {
            registerAccount = new RegisterAccount(configuration);
            userService = new UserService(configuration);
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAll([FromBody]Guid userId)
        {
            var user = registerAccount.GetAccountById(userId);
            return Ok(user);
        }
        [HttpPost("Login")]
        public IActionResult Login(Account account)
        {
            var token = userService.Authenticate(account.UserName, account.Password);
            if (token != null)
            {
                return Ok(token);
            }
            return Unauthorized();
        }
        [HttpPost("Register")]
        public IActionResult Register(Account account)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password, salt);
            var rowEffects = registerAccount.InsertAccount(account);
            if(rowEffects > 0)
                return Ok();
            return NoContent();
        }
    }
}
