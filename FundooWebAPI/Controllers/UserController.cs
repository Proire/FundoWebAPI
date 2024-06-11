using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Entity;
using UserRLL.Exceptions;

namespace FundooWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBLL;
        private readonly ResponseModel responeModel;
        public UserController(IUserBL userBLL, IConfiguration configuration) 
        {
            this.userBLL = userBLL;
            responeModel = new ResponseModel(); 
        }


        [HttpPost]
        [Route("/register")]
        public ResponseModel Register([FromBody] UserModel user)
        {
            try
            {
                UserModel model = userBLL.AddUser(user);
                responeModel.message = "User Added SuccessFully, Go to Login";
                responeModel.data = model.ToString();
            }
            catch (UserException ex)
            {
                responeModel.status = false;
                responeModel.message = ex.Message;
            }
            return responeModel;
        }


        [HttpPost]
        [Route("/login")]
        public ResponseModel Login([FromBody] LoginModel model)
        {
            try
            {
                UserEntity user = userBLL.Login(model);
                responeModel.message = "Loggedin Successfully!";
                responeModel.data = GenerateToken(user);
            }
            catch (UserException ex)
            {
                responeModel.status=false;
                responeModel.message = ex.Message;
            }
            return responeModel;
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity!=null)
            {
                var userClaims = identity.Claims;

                return new UserModel()
                {
                    UserName = userClaims.FirstOrDefault(o => o.Type==ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                    Name = userClaims.FirstOrDefault(o => o.Type==ClaimTypes.GivenName)?.Value ?? string.Empty,
                    PhoneNumber = userClaims.FirstOrDefault(o => o.Type==ClaimTypes.MobilePhone)?.Value ?? string.Empty,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value ?? string.Empty,
                };

            }
            return new UserModel()
            {
                UserName = string.Empty,
                Name = string.Empty,
                PhoneNumber = string.Empty,
                Role = string.Empty
            };

        }

        private static string GenerateToken(UserEntity user)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            // Retrieve issuer and audience from appsettings.json
            string issuer = configuration["JWT:ValidIssuer"] ?? string.Empty;
            string audience = configuration["JWT:ValidAudience"] ?? string.Empty;
           
            var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SecretKey")  ?? string.Empty));
            var credentials = new SigningCredentials(security,SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.GivenName,user.Name),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                new Claim(ClaimTypes.Role,user.Role),
            };
            var token = new JwtSecurityToken(issuer, audience,claims,expires:DateTime.Now.AddMinutes(15),signingCredentials:credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}