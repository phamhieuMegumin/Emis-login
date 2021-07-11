using Login.Demo.Web.Entities;
using Login.Demo.Web.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Login.Demo.Web.Service
{
    public class UserService
    {
        IConfiguration _configuration;
        RegisterAccount registerAccount;
        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
            registerAccount = new RegisterAccount();
        }
        public ResponseLogin Authenticate(string userName, string password)
        {
            var user = registerAccount.GetAccountdByUserName(userName);
            if(user != null)
            {
                if(BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    // Tạo token 
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var secretKey = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);
                    var tokenDecriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, userName)
                }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                        SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDecriptor);
                    return new ResponseLogin()
                    {
                        Token = tokenHandler.WriteToken(token),
                        UserInfo = user
                    };
                }
            }
            return null;
        }
    }
}
