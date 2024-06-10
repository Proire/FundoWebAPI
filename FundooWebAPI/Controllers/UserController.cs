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
        private readonly IConfiguration configuration;
        public UserController(IUserBL userBLL, IConfiguration configuration) 
        {
            this.userBLL = userBLL;
            responeModel = new ResponseModel(); 
            this.configuration = configuration;
        }


        [HttpPost]
        [Route("/register")]
        public ResponseModel Register([FromBody] UserModel user)
        {
            UserModel model = null;
            try
            {
                model = userBLL.AddUser(user);
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
            UserEntity user = null;
            try
            {
                user = userBLL.Login(model);
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

        [Authorize]   // Unauthorized users cannot access this
        [HttpGet]
        [Route("/getUser")]
        public ResponseModel GetUser()
        {
            var user = GetCurrentUser();
            responeModel.message = $"Accessed User info";
            responeModel.data= user.ToString();
            return responeModel;
        }

        [Authorize(Roles ="Admin")]   // Unauthorized users cannot access this
        [HttpGet]
        [Route("/getadmin")]
        public ResponseModel Get()
        {
            var user = GetCurrentUser();
            responeModel.message = $"Accessed Admin info";
            responeModel.data = user.ToString();
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
                    UserName = userClaims.FirstOrDefault(o => o.Type==ClaimTypes.NameIdentifier)?.Value,
                    Name = userClaims.FirstOrDefault(o => o.Type==ClaimTypes.GivenName)?.Value,
                    PhoneNumber = userClaims.FirstOrDefault(o => o.Type==ClaimTypes.MobilePhone)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                };

            }
            return null;

        }

        private static string GenerateToken(UserEntity user)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            // Retrieve issuer and audience from appsettings.json
            string issuer = configuration["JWT:ValidIssuer"];
            string audience = configuration["JWT:ValidAudience"];
           
            var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SecretKey")));
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