using System;
namespace MovieManager.Core.Models
{
    public enum Role { Guest, Reviewer, Manager, Admin }
    
    public class User
    {       
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        
        public string Token { get; set; }   
        
        public string ResetPasswordCode { get; set; }    
        public string ConfirmEmailCode { get; set; }    
    
    }
}

