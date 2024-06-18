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
        UserEntity AddUser(UserModel user);  // add 
        UserEntity LoginUser(LoginModel login);   // verify 
        Task<ICollection<UserEntity>> GetUsers();   // get all users 
        UserEntity GetUserById(int id);     // get user by id 
        UserEntity GetUserByEmail(string email);   // get user by email 
        void ResetPassword(int UserId, string password);  // reset password 
        Task<UserEntity> DeleteUser(int UserId);  // delete user 
        Task<UserEntity> UpdateUser(int UserId, UserModel user);   // update user 
    }
}
