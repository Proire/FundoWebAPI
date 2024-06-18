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

        public UserEntity GetUserByEmail(string email)
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

        public async Task<ICollection<UserEntity>> GetUsers()
        {
            return await userRll.GetUsers();
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

        public void ResetPassword(int userId, string password)
        { 
            try
            {
                userRll.ResetPassword(userId, password);
            }
            catch (UserException ie)
            {
                Console.WriteLine(ie.Message);
                throw;
            }
        }

        public async Task<UserEntity> DeleteUser(int userId)
        {
            try
            {
                return await userRll.DeleteUser(userId);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<UserEntity> UpdateUser(int UserId, UserModel model)
        {
            try
            {
                return await userRll.UpdateUser(UserId, model); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
