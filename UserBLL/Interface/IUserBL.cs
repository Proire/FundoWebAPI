using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModelLayer;

namespace UserBLL.Interface
{
    public interface IUserBL
    {
        UserModel AddUser(UserModel model);
    }
}
