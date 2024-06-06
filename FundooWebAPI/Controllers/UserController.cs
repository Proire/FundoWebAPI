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
        public UserController(IUserBL userBLL) 
        {
            this.userBLL = userBLL;
        }
        [HttpPost]
        public IActionResult AddUser([FromBody] UserModel user)
        {
            UserModel model = null;
            try
            {
                model = userBLL.AddUser(user);
            }
            catch(UserException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(model);
        }
    }
}
