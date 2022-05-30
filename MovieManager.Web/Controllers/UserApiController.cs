// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
//
// using MovieManager.Core.Models;
// using MovieManager.Data.Services;
// using MovieManager.Web.ViewModels;
//
// namespace MovieManager.Web.Controllers
// {
//
//     // ** This is a Demo WebAPI Controller provides User Login Using JWT Token **
//
//     [ApiController]    
//     [Route("api")]     
//     // set default auth scheme as we are using both cookie and jwt authentication
//     [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//     public class UserApiController : ControllerBase
//     {
//         private readonly IUserService _svc;       
//         private readonly IConfiguration _config; // jwt settings
//       
//         public UserApiController(IUserService service, IConfiguration _configuration)
//         {      
//             _config = _configuration;            
//             _svc = service;
//         }
//
//         // POST api/login
//         [AllowAnonymous]
//         [HttpPost("login")]
//         public ActionResult<User> Login(UserLoginViewModel login)        
//         {                     
//             var user = _svc.Authenticate(login.Email, login.Password);            
//             if (user == null)
//             {
//                 return BadRequest(new { message = "Email or Password are incorrect" });
//             }
//             // sign jwt token to use in secure api requests
//             var authUser = SignInJwt(user);
//
//             return Ok(authUser);
//         }  
//
//         // POST api/login
//         [AllowAnonymous]
//         [HttpPost("register")]
//         public ActionResult<User> Register(UserRegisterViewModel reg)        
//         {                     
//             var user = _svc.AddUser(reg.Name, reg.Email, reg.Password, reg.Role);
//             if (user == null)
//             {
//                 return BadRequest(new { message = "User Could not be Registered" });
//             }
//             // sign jwt token to use in secure api requests
//             var authUser = SignInJwt(user);
//
//             return Ok(authUser);
//         }  
//
//         // Sign user in using JWT authentication
//         private UserViewModel SignInJwt(User user)
//         {
//             return new UserViewModel
//             {
//                 Id = user.Id,
//                 Name = user.Name,
//                 Email = user.Email,              
//                 Role = user.Role,
//                 Token = AuthBuilder.BuildJwtToken(user, _config),
//             };
//         }     
//
//     }
// }