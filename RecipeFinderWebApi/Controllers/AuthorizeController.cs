using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RecipeFinderWabApi.Logic.Handlers;
using RecipeFinderWabApi.Logic.signInHandlers;
using RecipeFinderWebApi.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.Controllers
{
    [EnableCors("RFCorsPolicy")]
    [AllowAnonymous]
    [Route("api/[controller]/{action}")]
    public class AuthorizeController : Controller
    {
        private SignInHandler signInHandler;
        private UserHandler userHandler;

        public AuthorizeController(SignInHandler signInHandler, UserHandler userHandler)
        {
            this.signInHandler = signInHandler;
            this.userHandler = userHandler;
        }

        public IActionResult Index()
        {
            return Redirect(nameof(Login));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login(string returnUrl = null, string errorMessage = null)
        {
            LoginViewModel model = new LoginViewModel();

            if (returnUrl == null)
            {
                model.ReturnUrl = Url.Content("~/");
            }
            else model.ReturnUrl = returnUrl;

            if (errorMessage != null)
            {
                model.ErrorMessage = errorMessage;
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                string errorMessage = "Entered fields are invalid!";

                return RedirectToAction(nameof(Login), new { errorMessage });
            }

            string[] result = signInHandler.Login(model.Input.EmailOrUsername, model.Input.Password);

            if (result == null)
            {
                string errorMessage = "Either the name or password is incorrect!";

                return RedirectToAction(nameof(Login), new { errorMessage, returnUrl = model.ReturnUrl });
            }

            Trace.WriteLine("User '" + model.Input.EmailOrUsername + "' has logged in succesfully!");

            return Redirect(model.ReturnUrl + "?AccessToken=" + result[1] + "&RefreshToken=" + result[2]);
        }

        public IActionResult Register(string returnUrl = null, string errorMessage = null)
        {
            RegisterViewModel model = new RegisterViewModel();

            if (returnUrl == null)
            {
                model.ReturnUrl = Url.Content("~/");
            }
            else model.ReturnUrl = returnUrl;

            if (errorMessage != null)
            {
                model.ErrorMessage = errorMessage;
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = "Entered fields are invalid!";

                return RedirectToAction(nameof(Register), new { errorMessage });
            }

            if (model.Input.Password != model.Input.ConfirmPassword)
            {
                string errorMessage = "Passwords are not equal!";

                return RedirectToAction(nameof(Register), new { errorMessage });
            }

            if (model.Input.DOB > DateTime.Now.AddYears(-16))
            {
                string errorMessage = "You are not allowed to use this app under 16!";

                return RedirectToAction(nameof(Register), new { errorMessage });
            }

            if ((userHandler.GetByName(model.Input.Name)) != null)
            {
                string errorMessage = "Username already exists!";

                return RedirectToAction(nameof(Register), new { errorMessage });
            }

            if ((userHandler.GetByName(model.Input.Email)) != null)
            {
                string errorMessage = "A user with this email address already exists!";

                return RedirectToAction(nameof(Register), new { errorMessage });
            }

            var user = signInHandler.Register(model.Input.Name, model.Input.Email, model.Input.Password, model.Input.DOB);

            if (user == null)
            {
                string errorMessage = "Registration failed!";

                return RedirectToAction(nameof(Register), new { errorMessage, returnUrl = model.ReturnUrl });
            }
            else
            {
                signInHandler.Login(user.Name, model.Input.Password);

                Trace.WriteLine("New user '" + model.Input.Name + "' has registered in succesfully!");
                model.ReturnUrl = model.ReturnUrl == null ? "/UserHome/Index" : model.ReturnUrl;
                return Redirect(model.ReturnUrl);
            }
        }

        [HttpPost]
        public IActionResult ValidateAccesstoken([FromBody] string accessToken)
        {
            return Ok(true);
        }

        [HttpPost]
        public IActionResult RefreshAccesstoken([FromBody] string refreshToken)
        {
            return Ok(signInHandler.GetAccessToken(refreshToken));
        }

        [HttpGet("{accessToken}")]
        public IActionResult GetUserByAccesstoken(string accessToken)
        {
            if (String.IsNullOrEmpty(accessToken)) { return StatusCode(404); }

            return Ok(signInHandler.GetUserByAccessToken(accessToken));
        }
    }
}
