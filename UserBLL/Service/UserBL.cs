using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Services;
using UserRLL.Interface;

namespace UserBLL.Service
{
    public class UserBL : IUserBL
    {
        public readonly IUserRL userRll;

        public UserBL(IUserRL userRll)
        {
            this.userRll = userRll;
        }

        public UserModel AddUser(UserModel model)
        {
            UserModel userModel;
            try
            {
                userModel = userRll.AddUser(model);
            }
            catch(Exception ie)
            {
                Console.WriteLine(ie.Message);
                throw;
            }
            return userModel;
        }
    }
}
