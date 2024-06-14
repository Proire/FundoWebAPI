using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Fpe;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Entity;
using UserRLL.Exceptions;
using UserRLL.Utilities;

namespace FundooWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBLL;
        private readonly EmailSender _emailSender;
        public UserController( IUserBL userBLL, IConfiguration configuration, EmailSender emailSender)
        {
            this.userBLL = userBLL;
            _emailSender = emailSender;
        }


        [HttpPost]
        [Route("/register")]
        public ResponseModel<string> Register([FromBody] UserModel user)
        {
            try
            {
                UserEntity model = userBLL.AddUser(user);
               
                ResponseModel<string> responseModel = new ResponseModel<string>() { Data = model.ToString(), Message = "User Added SuccessFully, Go to Login" };
                return responseModel;
            }
            catch (UserException ex)
            {
                ResponseModel<string> responseModel = new ResponseModel<string>() { Data = null, Message = ex.Message, Status = false };
                return responseModel;
            }
        }


        [HttpPost]
        [Route("/login")]
        public ResponseModel<string> Login([FromBody] LoginModel model)
        {
            try
            {
                UserEntity user = userBLL.Login(model);
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = "Loggedin Successfully!", Data = GenerateToken(user) };
                return responseModel;
            }
            catch (UserException ex)
            {
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = ex.Message, Data = string.Empty, Status = false };
                return responseModel;
            }
        }

        [HttpPost]
        [Route("/forgetPassword")]
        public ResponseModel<string> ForgotPassword([FromBody] EmailModel email)
        {
            try
            {
                UserEntity user = userBLL.GetUserByEmail(email); // fetched user using userId
                var token = GenerateToken(user);
                var resetPasswordUrl = $"http://localhost:5288/swagger/index.html?token={token}";
                _emailSender.SendEmail(new EmailDTO() {To = user.Email,Subject="Reset Password",Body=resetPasswordUrl});
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = "Valid User", Data = token};
                return responseModel;
            }
            catch (UserException ex)
            {
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = ex.Message, Data = string.Empty, Status = false };
                return responseModel;
            }
        }

        [Authorize]
        [HttpPatch]
        [Route("/resetPassword")]
        public ResponseModel<string> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                userBLL.ResetPassword(UserId,resetPasswordDTO);
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = "Password Updated", Data = string.Empty };
                return responseModel;
            }
            catch (UserException ex)
            {
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = ex.Message, Data = string.Empty, Status = false };
                return responseModel;
            }
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
                new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id)),
                new Claim(ClaimTypes.GivenName,user.UserName),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Email,user.Email),
            };
            var token = new JwtSecurityToken(issuer, audience,claims,expires:DateTime.Now.AddMinutes(15),signingCredentials:credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}