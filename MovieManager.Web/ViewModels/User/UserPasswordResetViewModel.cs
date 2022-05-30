using System;
using System.ComponentModel.DataAnnotations;
using MovieManager.Core.Models;


namespace MovieManager.Web.ViewModels
{
    public class UserPasswordResetViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }
   
        [Required]       
        public string ResetPasswordCode { get; set; }

    }
}