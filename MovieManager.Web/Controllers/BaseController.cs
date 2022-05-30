using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MovieManager.Web.Controllers
{
    public enum AlertType { success, danger, warning, info }

    // Implements General functionality which is then accessible to any 
    // Controller inheriting from BaseController
    public class BaseController : Controller
    {
        // set alert message
        public void Alert(string message, AlertType type = AlertType.info)
        {
            TempData["Alert.Message"] = message;
            TempData["Alert.Type"] = type.ToString();
        }

        // return user identity ID if authenticated otherwise null
        public int GetSignedInUserId()
        {
            try
            {
                if (User.Identity.IsAuthenticated) {
                    // id stored as a string in the Sid claim - convert to an int and return
                    string sid = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value;
                    return Int32.Parse(sid);
                }
            }
            catch (FormatException) { }
            return 0;
        }
        
        // check if user us currently authenticated
        public bool IsAuthenticated() 
        {
            return User.Identity.IsAuthenticated;
        }
    }
 
}