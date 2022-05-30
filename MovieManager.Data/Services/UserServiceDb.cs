using System;
using System.Linq;
using System.Collections.Generic;

using MovieManager.Core.Models;
using MovieManager.Core.Security;
using MovieManager.Data.Repositories;
namespace MovieManager.Data.Services
{
    public class UserServiceDb : IUserService
    {
        private readonly DatabaseContext  ctx;

        public UserServiceDb(DatabaseContext _ctx)
        {
            ctx = _ctx; 
        }

        public UserServiceDb()
        {
            ctx = DatabaseContextFactory.CreateCtx(); 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

        // ------------------ User Related Operations ------------------------

        // retrieve list of Users
        public IList<User> GetUsers()
        {
            return ctx.Users.ToList();
        }

        // Retrive User by Id 
        public User GetUser(int id)
        {
            return ctx.Users.FirstOrDefault(s => s.Id == id);
        }

        // Add a new User checking a User with same email does not exist
        public User AddUser(string name, string email, string password, Role role)
        {     
            var existing = GetUserByEmail(email);
            if (existing != null)
            {
                return null;
            } 

            var user = new User
            {            
                Name = name,
                Email = email,
                Password = Hasher.CalculateHash(password), // can hash if required 
                Role = role              
            };
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return user; // return newly added User
        }

        // Delete the User identified by Id returning true if deleted and false if not found
        public bool DeleteUser(int id)
        {
            var s = GetUser(id);
            if (s == null)
            {
                return false;
            }
            ctx.Users.Remove(s);
            ctx.SaveChanges();
            return true;
        }

        // Update the User with the details in updated 
        public User UpdateUser(User updated)
        {
            // verify the User exists
            var User = GetUser(updated.Id);
            if (User == null)
            {
                return null;
            }
            // update the details of the User retrieved and save
            User.Name = updated.Name;
            User.Email = updated.Email;
            User.Password = Hasher.CalculateHash(updated.Password);  
            User.Role = updated.Role; 

            ctx.SaveChanges();          
            return User;
        }

        public User GetUserByEmail(string email, int? id=null)
        {
            return ctx.Users.FirstOrDefault(u => u.Email == email && ( id == null || u.Id != id));
        }

        public IList<User> GetUsersQuery(Func<User,bool> q)
        {
            return ctx.Users.Where(q).ToList();
        }

        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
            var user = GetUserByEmail(email);

            // Verify the user exists and Hashed User password matches the password provided
            return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
            //return (user != null && user.Password == password ) ? user: null;
        }
    
        public User GetUnConfirmedUser(string email, string code)
        {
            return ctx.Users.FirstOrDefault(u => u.Email == email && u.ConfirmEmailCode == code);
        }

        public User ConfirmUser(string email, string code)
        {
            var user = ctx.Users.FirstOrDefault(x => x.Email == email && x.ConfirmEmailCode == code);
            if (user == null)
            {
                return null;
            }
            // verify email by removing ConfirmEmailCode
            user.ConfirmEmailCode = null;
            ctx.Update(user);
            ctx.SaveChanges();
            return user;     
        }

        public User ResetPassword(string userName, string newPassword, string ResetPasswordCode)
        {
            var u = ctx.Users.FirstOrDefault(x => x.Email == userName && x.ResetPasswordCode == ResetPasswordCode);
            if (u == null)
            {
                return null; // no such user or invalid ResetPasswordCode
            }

            // update the password and clear the ResetPasswordCode
            u.Password = Hasher.CalculateHash(newPassword);
            u.ResetPasswordCode = null;
            ctx.Update(u);
            ctx.SaveChanges();
            return u;
        }

        public User GenerateResetPasswordCode(string userName)
        {
            var u = ctx.Users.FirstOrDefault(x => x.Email == userName);
            if (u == null)
            {
                return null; // no such user 
            } 
            u.ResetPasswordCode = Guid.NewGuid().ToString();
            ctx.Update(u);
            ctx.SaveChanges();

            return u;
        }
   
    }
}