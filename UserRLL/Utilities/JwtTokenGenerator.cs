using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserRLL.Entity;

namespace UserRLL.Utilities
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(UserEntity user)
        {

            // Retrieve issuer and audience from appsettings.json
            string issuer = _configuration["JWT:ValidIssuer"] ?? string.Empty;
            string audience = _configuration["JWT:ValidAudience"] ?? string.Empty;

            var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SecretKey") ?? string.Empty));
            var credentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id)),
                new Claim(ClaimTypes.GivenName,user.UserName),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Email,user.Email),
            };
            var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
