using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using MovieManager.Data.Services;
using MovieManager.Core.Models;
using MovieManager.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using MovieManager.Core.Security;

/**
 *  User Management Controller providing registration and login functionality
 */
namespace MovieManager.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly IUserService _svc;
        private readonly IEmailService _mail;

        public UserController(IUserService svc, IEmailService mail, IConfiguration config)
        {        
            _config = config;
            _mail = mail;    
            _svc = svc;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] UserLoginViewModel m)
        {
            var user = _svc.Authenticate(m.Email, m.Password);
            // check if login was unsuccessful and add validation errors
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid Login Credentials");
                ModelState.AddModelError("Password", "Invalid Login Credentials");
                return View(m);
            }

            // Login Successful, so sign user in using cookie authentication
            await SignInCookie(user);

            Alert("Successfully Logged in", AlertType.info);

            return Redirect("/");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Name,Email,Password,PasswordConfirm,Role")] UserRegisterViewModel m)       
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            // add user via service
            var user = _svc.AddUser(m.Name, m.Email,m.Password, m.Role);
            // check if error adding user and display warning
            if (user == null) {
                Alert("There was a problem Registering. Please try again", AlertType.warning);
                return View(m);
            }

            Alert("Successfully Registered. Now login", AlertType.info);
            _mail.Send(m.Email,"Confirm Email","Please confirm email account");

            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult UpdateProfile()
        {
           // use BaseClass helper method to retrieve Id of signed in user 
            var user = _svc.GetUser(GetSignedInUserId());
            var userViewModel = new UserProfileViewModel { 
                Id = user.Id, 
                Name = user.Name, 
                Email = user.Email,                 
                Role = user.Role
            };
            return View(userViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile([Bind("Id,Name,Email,Role")] UserProfileViewModel m)       
        {
            var user = _svc.GetUser(m.Id);
            // check if form is invalid and redisplay
            if (!ModelState.IsValid || user == null)
            {
                return View(m);
            } 

            // update user details and call service
            user.Name = m.Name;
            user.Email = m.Email;
            user.Role = m.Role;        
            var updated = _svc.UpdateUser(user);

            // check if error updating service
            if (updated == null) {
                Alert("There was a problem Updating. Please try again", AlertType.warning);
                return View(m);
            }

            Alert("Successfully Updated Account Details", AlertType.info);
            
            // sign the user in with updated details)
            await SignInCookie(user);

            return RedirectToAction("Index","Home");
        }

        // Change Password
        [Authorize]
        public IActionResult UpdatePassword()
        {
            // use BaseClass helper method to retrieve Id of signed in user 
            var user = _svc.GetUser(GetSignedInUserId());
            var passwordViewModel = new UserPasswordChangeViewModel { 
                Id = user.Id, 
                Password = user.Password, 
                PasswordConfirm = user.Password, 
            };
            return View(passwordViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword([Bind("Id,OldPassword,Password,PasswordConfirm")] UserPasswordChangeViewModel m)       
        {
            var user = _svc.GetUser(m.Id);
            if (!ModelState.IsValid || user == null)
            {
                return View(m);
            }  
            // update the password
            user.Password = m.Password; 
            // save changes      
            var updated = _svc.UpdateUser(user);
            if (updated == null) {
                Alert("There was a problem Updating the password. Please try again", AlertType.warning);
                return View(m);
            }

            Alert("Successfully Updated Password", AlertType.info);
            // sign the user in with updated details
            await SignInCookie(user);

            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
  
        [HttpPost]
        public IActionResult ForgotPassword(UserForgotPasswordViewModel req)
        {
            // generate reset password code
            var user = _svc.GenerateResetPasswordCode(req.Email);
                  
            if (user != null)
            {
                // generate email body
                var body = GenerateResetMessage(user);

                // send the email using email service       
                if (!_mail.Send(user.Email, "Reset Password", body))
                {
                    Alert("Problem Sending Reset Password Email", AlertType.warning);   
                    return View();                     
                } 
                Alert("Reset Password Email Sent", AlertType.info);           
            }
            return RedirectToAction(nameof(ResetPassword), new {Email = user.Email});                
        }

        public IActionResult ResetPassword(string email = null, string code = null)
        {
            // display view to enter userName (email), new password and resetPasswordCode
            return View(new UserPasswordResetViewModel { Email = email, ResetPasswordCode = code });
        }
            
        [HttpPost]
        public IActionResult ResetPassword(UserPasswordResetViewModel reset)
        {
            // reset the password
            if (_svc.ResetPassword(reset.Email, reset.Password, reset.ResetPasswordCode) != null)
            {
                Alert("Password Reset. Login using new password", AlertType.info);
                return RedirectToAction(nameof(Login));
            }

            // invalid reset credentials so re-display the view
            ModelState.AddModelError("Email", "Invalid Reset Password Credentials");
            ModelState.AddModelError("ResetPasswordCode", "Invalid Reset Password Credentials");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        // Return not authorised and not authenticated views
        public IActionResult ErrorNotAuthorised() => View();
        public IActionResult ErrorNotAuthenticated() => View();

        // -------------------------- Helper Methods ------------------------------

        // Called by Remote Validation attribute on RegisterViewModel to verify email address is unique
        [AcceptVerbs("GET", "POST")]
        public IActionResult GetUserByEmailAddress(string email, int id)
        {
            // check if email is available, unless already owned by user with id
            var user = _svc.GetUserByEmail(email, id);
            if (user != null)
            {
                return Json($"A user with this email address {email} already exists.");
            }
            return Json(true);                  
        }

        // Called by Remote Validation attribute on ChangePassword to verify old password
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPassword(string oldPassword)
        {
            // use BaseClass helper method to retrieve Id of signed in user 
            var id = GetSignedInUserId();            
            // check if email is available, unless already owned by user with id
            var user = _svc.GetUser(id);
            if (user == null || !Hasher.ValidateHash(user.Password, oldPassword))
            {
                return Json($"Please enter current password.");
            }
            return Json(true);                  
        }

        // ----------------------- Private Utility Methods ----------------

        // Sign user in using Cookie authentication scheme
        private async Task SignInCookie(User user)
        {
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                AuthBuilder.BuildClaimsPrincipal(user)
            );
        }

        // Generate a Url based on current Request 
        private string GenerateUrl(string req)
        {
            return Request.Scheme + "://" + Request.Host.Value + req;                      
        }

        private string GenerateResetMessage(User user)
        {
            var url = GenerateUrl($"/User/ResetPassword?email={user.Email}&code={user.ResetPasswordCode}");
            var body = $"Your reset password code is {user.ResetPasswordCode} \n {url}";
            return body;
        }
    }
}