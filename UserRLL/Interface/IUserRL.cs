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
        UserModel AddUser(UserModel user);
        UserEntity LoginUser(LoginModel login);

        IEnumerable<UserEntity> GetUsers();
    }
}
