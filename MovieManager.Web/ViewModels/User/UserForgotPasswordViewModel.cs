using System;
using System.ComponentModel.DataAnnotations;

namespace MovieManager.Web.ViewModels
{
    public class UserForgotPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
        
    }
}