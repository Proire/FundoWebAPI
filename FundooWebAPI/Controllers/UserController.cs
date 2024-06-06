using Microsoft.AspNetCore.Mvc;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Exceptions;

namespace FundooWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBLL;
        private readonly ResponseModel responeModel;
        public UserController(IUserBL userBLL) 
        {
            this.userBLL = userBLL;
            responeModel = new ResponseModel(); 
        }
        [HttpPost]
        public ResponseModel RegisterUser([FromBody] UserModel user)
        {
            UserModel model = null;
            try
            {
                model = userBLL.AddUser(user);
                responeModel.message = "User Added SuccessFully";
                responeModel.data = model.ToString();
            }
            catch(UserException ex)
            {
                responeModel.status = false;
                responeModel.message = ex.Message;
            }
            return responeModel;
        }
    }
}
