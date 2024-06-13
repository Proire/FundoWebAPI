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
        UserEntity AddUser(UserModel user);
        UserEntity LoginUser(LoginModel login);

        ICollection<UserEntity> GetUsers();

        UserEntity GetUserById(int id);

        UserEntity GetUserByEmail(EmailModel email);

        void VerifiedEmail(int UserId);

        void ResetPassword(int UserId, ResetPasswordDTO resetPasswordDTO);
    }
}
