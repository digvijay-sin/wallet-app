using Expense_Tracker_Core.Models;
using Expense_Tracker_Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Expense_Tracker_API.Service
{
    public class TokenGenerator
    {
        private readonly IConfiguration _config;

        public TokenGenerator(IConfiguration config)
        {
            _config = config;
        }
        public UserRes User { get; set; }
        public string Token
        {
            get
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, User.UserId.ToString()),
                    new Claim(ClaimTypes.Name, User.Username)
                };

                var tokenDescriptor = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    signingCredentials: cred,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60)
                );

                var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
                return token;
            }
        }
    }
}
