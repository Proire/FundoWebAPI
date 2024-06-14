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
using System.Text.RegularExpressions;
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
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        public UserController( IUserBL userBLL, IConfiguration configuration, EmailSender emailSender, JwtTokenGenerator jwtTokenGenerator)
        {
            this.userBLL = userBLL;
            _emailSender = emailSender;
            _jwtTokenGenerator = jwtTokenGenerator;
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
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = "LoggedIn Successfully!", Data = _jwtTokenGenerator.GenerateToken(user) };
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
        public ResponseModel<string> ForgotPassword([FromBody] string email)
        {
            try
            {
                bool isValid = Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if(!isValid)
                {
                    return new ResponseModel<string>() { Message = "Please Enter Valid Email", Data = string.Empty, Status = false };
                }

                UserEntity user = userBLL.GetUserByEmail(email);
                var token = _jwtTokenGenerator.GenerateToken(user);
                _emailSender.SendEmail(new EmailDTO() {To = user.Email,Subject="Reset Password",Body=token});
                return new ResponseModel<string>() { Message = "Valid User", Data = token};
            }
            catch (UserException ex)
            {
                return new ResponseModel<string>() { Message = ex.Message, Data = string.Empty, Status = false };
            }
        }

        [Authorize]
        [HttpPatch]
        [Route("/resetPassword")]
        public ResponseModel<string> ResetPassword([FromBody] string password)
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                userBLL.ResetPassword(UserId,password);
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = "Password Updated", Data = string.Empty };
                return responseModel;
            }
            catch (UserException ex)
            {
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = ex.Message, Data = string.Empty, Status = false };
                return responseModel;
            }
        }
    }
}