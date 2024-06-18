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

        public UserEntity GetUserByEmail(string email)
        {
            try
            {
                var user = Context.Users.FirstOrDefault(p => p.Email == email);
                if (user != null)
                    return user;
                throw new UserException($"No User Found with email : {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ICollection<UserEntity>> GetUsers()
        {
            try
            {

                var users = await Context.Users.ToListAsync();
                if(users.Count > 0)
                    return users;
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
                throw new UserException("Wrong Password, Reenter Password");
            }
            
            throw new UserException("Invalid UserName, Register First");
            
        }

        public void ResetPassword(int UserId, string password)
        {
            try
            {
                var existingUser = Context.Users.FirstOrDefault(p => p.Id == UserId);
                if (existingUser != null)
                {
                    existingUser.Password = PasswordHasher.HashPassword(password);
                    Context.SaveChanges();
                }
                else
                {
                    throw new UserException($"No User Found with id : {UserId}");
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"An error occurred while updating Note with ID : {UserId}");
                throw;
            }
        }

        public async Task<UserEntity> DeleteUser(int UserId)
        {
            try
            {
                var existingUser =  await Context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
                if (existingUser == null)
                    throw new UserException($"User with id {UserId} not found");
                Context.Users.Remove(existingUser);
                await Context.SaveChangesAsync();
                return existingUser;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<UserEntity> UpdateUser(int UserId, UserModel user)
        {
            try
            {
                var existingUser = await Context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
                if (existingUser == null)
                    throw new UserException($"User with id {UserId} not found");
                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password;
                existingUser.Email = user.Email;
                existingUser.Name = user.Name;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.Role = user.Role;

                Context.Users.Update(existingUser);   
                await Context.SaveChangesAsync();
                return existingUser;
            }
            catch(Exception e ) 
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
