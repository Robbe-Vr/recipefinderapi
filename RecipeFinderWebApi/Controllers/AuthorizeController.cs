using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.UI.Auth;
using RecipeFinderWebApi.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using RecipeFinderWebApi.UI.Filters;

namespace RecipeFinderWebApi.UI.Controllers
{
    [EnableCors("RFCorsPolicy")]
    [AllowAnonymous]
    [Route("api/[controller]/{action}")]
    public class AuthorizeController : Controller
    {
        private readonly UserHandler userHandler;
        private readonly SignInHandler signInHandler;

        public AuthorizeController(UserHandler userHandler, SignInHandler signInHandler)
        {
            this.userHandler = userHandler;
            this.signInHandler = signInHandler;
        }

        public IActionResult Index()
        {
            return Redirect(nameof(Login));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login(string returnUrl = null)
        {
            LoginViewModel model = new LoginViewModel();

            if (returnUrl == null)
            {
                model.Input = new LoginViewModel.LoginCredentials() { ReturnUrl = Url.Content("~/") };
            }
            else model.Input = new LoginViewModel.LoginCredentials() { ReturnUrl = returnUrl };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model, bool cancel = false)
        {
            if (cancel)
            {
                return Redirect(model.Input.ReturnUrl ?? Url.Content("~/"));
            }
            
            if (!ModelState.IsValid)
            {
                string errorMessage = "Entered fields are invalid!";

                return RedirectToAction(nameof(Login), new { errorMessage });
            }

            User user = signInHandler.Login(model.Input.EmailOrUsername, model.Input.Password);
            Trace.WriteLine("User '" + model.Input.EmailOrUsername + "' has logged in succesfully!");

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Either name or password is incorrect.");

                return View(model);
            }

            if (!String.IsNullOrEmpty(model.Input.ReturnUrl))
            {
                return Redirect(model.Input.ReturnUrl + "?Token=" + signInHandler.GetToken(user.Id) + "&UserId=" + user.Id);
            }

            ModelState.AddModelError(String.Empty, "Invalid return url! exit this website manually.");

            return View(model);
        }

        public IActionResult Register(string returnUrl = null)
        {
            RegisterViewModel model = new RegisterViewModel();

            if (returnUrl == null)
            {
                model.ReturnUrl = Url.Content("~/");
            }
            else model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = "Entered fields are invalid!";

                ModelState.AddModelError(String.Empty, errorMessage);

                return View(model);
            }

            if (model.Input.Password != model.Input.ConfirmPassword)
            {
                string errorMessage = "Passwords are not equal!";

                ModelState.AddModelError(String.Empty, errorMessage);

                return View(model);
            }

            if (model.Input.DOB > DateTime.Now.AddYears(-16))
            {
                string errorMessage = "You are not allowed to use this app under 16!";

                ModelState.AddModelError(String.Empty, errorMessage);

                return View(model);
            }

            if ((userHandler.GetByName(model.Input.Name)) != null)
            {
                string errorMessage = "Username already exists!";

                ModelState.AddModelError(String.Empty, errorMessage);

                return View(model);
            }

            if ((userHandler.GetByName(model.Input.Email)) != null)
            {
                string errorMessage = "A user with this email address already exists!";

                ModelState.AddModelError(String.Empty, errorMessage);

                return View(model);
            }

            var newUser = signInHandler.Register(model.Input.Name, model.Input.Email, model.Input.Password, model.Input.DOB);

            if (newUser == null)
            {
                string errorMessage = "Registration failed!";

                ModelState.AddModelError(String.Empty, errorMessage);

                return View(model);
            }
            else
            {
                User user = signInHandler.Login(newUser.Name, model.Input.Password);

                if (user == null)
                {
                    return RedirectToAction(nameof(Login), new { returnUrl = model.ReturnUrl });
                }

                Trace.WriteLine("New user '" + model.Input.Name + "' has registered in succesfully!");

                if (!String.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl + "?Token=" + signInHandler.GetToken(user.Id) + "&UserId=" + user.Id);
                }

                ModelState.AddModelError(String.Empty, "Invalid return url! exit this website manually.");

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Logout(string logoutId, bool showLogoutPrompt = false, string returnUrl = null)
        {
            var model = new LogoutModel()
            {
                ReturnUrl = returnUrl,
                LogoutId = logoutId,
            };

            if (showLogoutPrompt == false)
            {
                return Logout(model);
            }

            return View(model);
        }

        [HttpPost]
        [RequiresRoles(true, "Default")]
        public IActionResult Logout(LogoutModel model)
        {
            string accessToken = HttpContext.Request.Headers["RecipeFinder_AccessToken"].ToString();

            if (!String.IsNullOrEmpty(accessToken))
            {
                signInHandler.Logout(accessToken);
            }

            if (model.TriggerExternalSignout)
            {
                string url = Url.Action("Logout", new { logoutId = model.LogoutId });

                return SignOut(new AuthenticationProperties { RedirectUri = url }, model.ExternalAuthenticationScheme);
            }

            if (!String.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [RequiresRoles(true, "Default")]
        public IActionResult Validate()
        {
            string accessToken = HttpContext.Request.Headers["RecipeFinder_AccessToken"].ToString();

            string result = signInHandler.ValidateToken(accessToken);

            if (result == "Not found.")
            {
                return StatusCode(200, new { Message = "Unknown access token. No user is signed in with this token.", Result = "Not found." });
            }
            else if (result == "Expired.")
            {
                return Ok(new { Message = "Access token is expired. Send a post request to '/refresh' with your current access token or sign in again.", Result = "Expired." });
            }
            else
            {
                return Ok(new { Result = "Success." });
            }
        }

        [HttpPost]
        public IActionResult Refresh()
        {
            string accessToken = HttpContext.Request.Headers["RecipeFinder_AccessToken"].ToString();

            var result = signInHandler.RefreshToken(accessToken);

            if (String.IsNullOrEmpty(result))
            {
                return StatusCode(500, new { Message = "Error occurred refreshing access token." });
            }
            else if (result == "Unkown access token.")
            {
                return StatusCode(404, new { Message = result });
            }
            else
            {
                return Ok(new { access_token = result });
            }
        }

        [HttpGet]
        [RequiresRoles(true, "Default")]
        public IActionResult Me()
        {
            string accessToken = HttpContext.Request.Headers["RecipeFinder_AccessToken"].ToString();

            User user = signInHandler.GetUserByToken(accessToken);

            if (user == null)
            {
                return StatusCode(404, new { Message = "No user found attached to access token." });
            }
            else
            {
                return Ok(user);
            }
        }
        
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

