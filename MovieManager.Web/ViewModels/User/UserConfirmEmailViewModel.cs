using System;
using System.ComponentModel.DataAnnotations;

namespace MovieManager.Web.ViewModels
{
    public class UserConfirmEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
     
        [Required]
        public string ConfirmEmailCode { get; set; }
    }
}