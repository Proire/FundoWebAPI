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
    }
}
