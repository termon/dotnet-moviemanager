using System;
using System.Net;
using System.ComponentModel.DataAnnotations;

using MovieManager.Web.ViewModels;

namespace MovieManager.Web.Validators
{
    public class UrlValidator : ValidationAttribute   
    {
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //MovieViewModel movie = (MovieViewModel)validationContext.ObjectInstance;

            string _url = (string)value; // movie.PosterUrl;
            
            // if a url is provided then verify it represents a valid resource
            var validURL = Uri.IsWellFormedUriString(_url, UriKind.Absolute);
            
            if (validURL)
            {
                var uri = new Uri(_url, UriKind.Absolute);

                // using method head doesn't down load the resource, rather it just verifies its existence
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Method = "HEAD";
                try
                {
                    webRequest.GetResponse();
                    return ValidationResult.Success;
                }
                catch
                {
                    return new ValidationResult("Url Endpoint is not valid");
                }
            } 
            return new ValidationResult("Url is not valid");                
        }
   
    }
}