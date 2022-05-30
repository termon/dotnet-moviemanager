using System;
using System.ComponentModel.DataAnnotations;

// https://github.com/cornflourblue/aspnet-core-registration-login-api

namespace MovieManager.Web.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}