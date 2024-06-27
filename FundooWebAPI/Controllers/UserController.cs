using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
    public class UserController(IUserBL userBLL, EmailSender emailSender, JwtTokenGenerator jwtTokenGenerator, ICacheService cacheService) : ControllerBase
    {
        private readonly IUserBL userBLL = userBLL;
        private readonly EmailSender _emailSender = emailSender;
        private readonly JwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
        private readonly ICacheService _cacheService = cacheService;

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

        [HttpPut]
        [Route("/updateUser/{id}")]
        public async Task<ResponseModel<UserEntity>> UpdateUser(int id, [FromBody] UserModel model)
        {
            try
            {
                var updateUser = await userBLL.UpdateUser(id, model);
                return new ResponseModel<UserEntity>() { Message = "User Updated Successfully", Data = updateUser };
            }
            catch (UserException e)
            {
                return new ResponseModel<UserEntity>() { Status = false, Message = e.Message, Data = null };
            }
        }

        [HttpDelete]
        [Route("/deleteUser/{id}")]
        public async Task<ResponseModel<UserEntity>> DeleteUser(int id)
        {
            try
            {
                var updateUser = await userBLL.DeleteUser(id);
                return new ResponseModel<UserEntity>() { Message = "User Deleted Successfully", Data = updateUser };
            }
            catch (UserException e)
            {
                return new ResponseModel<UserEntity>() { Status = false, Message = e.Message, Data = null };
            }
        }


        [HttpPost]
        [Route("/login")]
        public ResponseModel<string> Login([FromBody] LoginModel model)
        {
            try
            {
                UserEntity user = userBLL.Login(model);
                // Using Session State Management - Setting the UserId
                HttpContext.Session.SetInt32("UserId", user.Id);

                // token generated for further logged in
                var token = _jwtTokenGenerator.GenerateCrudToken(Convert.ToString(user.Id), user.UserName, TimeSpan.FromMinutes(15));
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = "LoggedIn Successfully!", Data = token };
                return responseModel;
            }
            catch (UserException ex)
            {
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = ex.Message, Data = string.Empty, Status = false };
                return responseModel;
            }
        }

        [HttpGet]
        [Route("/user/{id}")]
        public ResponseModel<UserEntity> GetUser(int id)
        {
            try
            {
                var User = userBLL.GetUserById(id);
                return new ResponseModel<UserEntity>() { Message = "User retrieved Successfully", Data = User };
            }
            catch (UserException e)
            {
                return new ResponseModel<UserEntity>() { Status = false, Message = e.Message, Data = null };
            }
        }

        [HttpGet]
        [Route("/users")]
        public async Task<ResponseModel<IEnumerable<UserEntity>>> GetUsers()
        {
            try
            {
                var users = await userBLL.GetUsers();
                return new ResponseModel<IEnumerable<UserEntity>>() { Message = "Users Retrieved Successfully", Data = users };
            }
            catch (UserException)
            {
                return new ResponseModel<IEnumerable<UserEntity>>() { Status = false, Message = "Problem Occured while retrieving users", Data = null };
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
                var token = _jwtTokenGenerator.GenerateUserValidationToken(Convert.ToString(user.Id),TimeSpan.FromMinutes(15));
                _emailSender.SendEmail(new EmailDTO() {To = user.Email,Subject="Reset Password",Body=token});
                return new ResponseModel<string>() { Message = "Valid User", Data = token};
            }
            catch (UserException ex)
            {
                return new ResponseModel<string>() { Message = ex.Message, Data = string.Empty, Status = false };
            }
        }

        [Authorize(AuthenticationSchemes = "UserValidationScheme")]
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