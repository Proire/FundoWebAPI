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
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _secretKey = Environment.GetEnvironmentVariable("SecretKey") ?? string.Empty;
            _issuer = configuration["JWT:ValidIssuer"] ?? string.Empty;
            _audience = configuration["JWT:ValidAudience"] ?? string.Empty;
        }

        public string GenerateCrudToken(string userId, string userName, TimeSpan tokenExpiry)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(userId)),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "crud_user")
            };

            return GenerateToken(claims, tokenExpiry);
        }

        public string GenerateUserValidationToken(string userId, TimeSpan tokenExpiry)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(userId)),
                new Claim("purpose", "user_validation")
            };

            return GenerateToken(claims, tokenExpiry);
        }

        public string GenerateEmailVerificationToken(string otp, TimeSpan tokenExpiry)
        {
            var claims = new[]
            {
                new Claim("otp", otp),
                new Claim("purpose", "email_verification")
            };

            return GenerateToken(claims, tokenExpiry);
        }

        private string GenerateToken(Claim[] claims, TimeSpan tokenExpiry)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.Add(tokenExpiry),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
