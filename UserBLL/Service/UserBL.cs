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

        public UserEntity AddUser(UserModel model)
        {
            try
            {
                return userRll.AddUser(model);
            }
            catch(Exception ie)
            {
                Console.WriteLine(ie.Message);
                throw;
            }
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

        public UserEntity GetUserByEmail(EmailModel email)
        {

            try
            {
                return userRll.GetUserByEmail(email);
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
            try
            {
                userRll.ResetPassword(userId, resetPasswordDTO);
            }
            catch (UserException ie)
            {
                Console.WriteLine(ie.Message);
                throw;
            }
        }

        public void VerifyEmail(int userId)
        {
            try
            {
                userRll.VerifiedEmail(userId);
            }
            catch (UserException ie)
            {
                Console.WriteLine(ie.Message);
                throw;
            }
        }
    }
}
