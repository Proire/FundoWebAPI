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
using Microsoft.EntityFrameworkCore;

namespace UserRLL.Services
{
    public class UserRL(UserDBContext context) : IUserRL
    {
        private readonly UserDBContext Context = context;

        public UserEntity AddUser(UserModel user)
        {
            // Hashing Password before storing in datasource
            user.Password = PasswordHasher.HashPassword(user.Password);
            UserEntity userEntity = new UserEntity() { Name=user.Name,UserName = user.UserName, Password=user.Password,PhoneNumber=user.PhoneNumber,Role=user.Role, Email=user.Email};
            try
            {
                Context.Users.Add(userEntity);
                Context.SaveChanges(); 
            }
            catch(Exception ie)
            {
                Console.WriteLine(ie.Message);
                throw;
            }
            return userEntity;
        }

        public UserEntity GetUserById(int id)
        {
            try
            {
                var user = Context.Users.FirstOrDefault(p => p.Id == id);
                if (user != null)
                    return user;
                throw new UserException($"No User Found with id : {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public UserEntity GetUserByEmail(EmailModel email)
        {
            try
            {
                var user = Context.Users.FirstOrDefault(p => p.Email == email.Email);
                if (user != null)
                    return user;
                throw new UserException($"No User Found with email : {email.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public ICollection<UserEntity> GetUsers()
        {
            try
            {
                var Users = Context.Users.ToList();
                if(Users.Count>0)
                    return Users;
                throw new UserException("No Users Found");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
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
                throw new UserException("Wrong Password, ReEnter Password");
            }
            
            throw new UserException("Invalid UserName, Register First");
            
        }

        public void ResetPassword(int UserId, ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var existingUser = Context.Users.FirstOrDefault(p => p.Id == UserId);
                if (existingUser != null)
                {
                    existingUser.Password = PasswordHasher.HashPassword(resetPasswordDTO.Password);
                    Context.SaveChanges();
                }
                throw new UserException($"No User Found with id : {UserId}");
            }
            catch (Exception)
            {
                Console.WriteLine($"An error occurred while updating Note with ID : {UserId}");
                throw;
            }
        }

        public void VerifiedEmail(int UserId)
        {
            try
            {
                var existingUser = Context.Users.FirstOrDefault(p => p.Id == UserId);
                if (existingUser != null)
                {
                    if (!existingUser.IsEmailVerified)
                    {
                        existingUser.IsEmailVerified = true;
                        Context.SaveChanges();
                    }
                    else
                        throw new UserException("Email Verified Already");
                }
                else
                    throw new UserException($"No User Found with id : {UserId}");
            }
            catch (Exception)
            {
                Console.WriteLine($"An error occurred while updating Note with ID : {UserId}");
                throw;
            }
        }
    }
}
