using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModelLayer;
using UserRLL.Entity;

namespace UserBLL.Interface
{
    public interface IUserBL
    {
        UserEntity AddUser(UserModel model);
        UserEntity Login(LoginModel login);
        Task<ICollection<UserEntity>> GetUsers();
        UserEntity GetUserById(int id);

        UserEntity GetUserByEmail(string email);

        void ResetPassword(int userId, string password);

        Task<UserEntity> DeleteUser(int userId); 

        Task<UserEntity> UpdateUser(int userId, UserModel model);
    }
}
