using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModelLayer;
using UserRLL.Entity;

namespace UserRLL.Interface
{
    public interface IUserRL
    {
        // Register User 
        UserEntity AddUser(UserModel user);

        // Validate User 
        UserEntity LoginUser(LoginModel login);
        ICollection<UserEntity> GetUsers();
        UserEntity GetUserById(int id);
        UserEntity GetUserByEmail(string email);
        void ResetPassword(int UserId, string password);
    }
}
