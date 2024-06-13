using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Services;
using UserRLL.Interface;
using UserRLL.Entity;
using UserRLL.Exceptions;

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

        public UserEntity GetUserById(int id)
        {
           
            try
            {
                return userRll.GetUserById(id);
            }
            catch (UserException ie)
            {
                Console.WriteLine(ie.Message);
                throw;
            }
        }

        public ICollection<UserEntity> GetUsers()
        {
            return userRll.GetUsers();
        }

        public UserEntity Login(LoginModel login)
        {
            try
            {
                return userRll.LoginUser(login);
            }
            catch(UserException ie)
            {
                Console.WriteLine(ie.Message);
                throw;
            }
        }

        public void ResetPassword(int userId, ResetPasswordDTO resetPasswordDTO)
        {
            userRll.ResetPassword(userId, resetPasswordDTO);
        }
    }
}
