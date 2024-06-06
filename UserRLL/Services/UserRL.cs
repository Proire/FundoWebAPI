using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModelLayer;
using UserRLL.Context;
using UserRLL.Entity;
using UserRLL.Interface;
using UserRLL.Exceptions;
using UserRLL.Utilities;

namespace UserRLL.Services
{
    public class UserRL : IUserRL
    {
        private readonly UserDBContext Context;
        public UserRL(UserDBContext context) { 
            this.Context = context;
        }

        public UserModel AddUser(UserModel user)
        {
            user.Password = PasswordHasher.HashPassword(user.Password);
            UserEntity userEntity = new UserEntity() { Name=user.Name,UserName = user.UserName, Password=user.Password,PhoneNumber=user.PhoneNumber};
            try
            {
                Context.Users.Add(userEntity);
                Context.SaveChanges();
            }
            catch(Exception ie)
            {
                Console.WriteLine(ie.Message);
                throw new UserException("Problem While Adding User");
            }
            return user;
        }

        public UserEntity LoginUser(LoginModel model)
        {
            bool isuser = Context.Users.Any(x=>x.UserName==model.UserName);
            if (isuser)
            {
                UserEntity? user = Context.Users.FirstOrDefault(x => x.UserName == model.UserName);
                if (user != null && PasswordHasher.VerifyPassword(model.Password,user.Password)) 
                { 
                    return user; 
                }
                else { throw new UserException("Wrong Password, ReEnter Password"); }
            }
            else
            {
                throw new UserException("Invalid UserName, Register First");
            }
        }
    }
}
