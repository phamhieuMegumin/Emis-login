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
using System.Security.Claims;
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
        [HttpGet("{accountId}")]
        public IActionResult GetAll(Guid accountId)
        {
            var user = registerAccount.GetAccountById(accountId);
            return Ok(user);
        }
        [Authorize]
        [HttpGet("Authentication")]
        public IActionResult CheckAuthentication()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var user = registerAccount.GetAccountById(Guid.Parse(userId));
            return Ok(user);
        }
        [Authorize]
        [HttpGet("Courses")]
        public IActionResult GetCourses()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var courses = registerAccount.GetCourses(Guid.Parse(userId));
            return Ok(courses);
        }
        [Authorize]
        [HttpPost("Course")]
        public IActionResult InsertCourse(Course course)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            course.AccountId = Guid.Parse(userId);
            var rowEffects = registerAccount.InsertCourse(course);
            if(rowEffects > 0)
            {
                return Ok();
            }
            return NoContent();
        }
        [HttpPost("Login")]
        public IActionResult Login(Account account)
        {
            var token = userService.Authenticate(account.UserName, account.Password);
            if (token != null)
            {
                //Response.Cookies.Append("JWT", token.Token, new CookieOptions { IsEssential = true});
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
