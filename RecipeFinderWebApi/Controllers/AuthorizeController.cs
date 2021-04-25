using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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

            return Redirect(model.ReturnUrl + "?Code=" + result[0] + "&UserId=" + result[1]);
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
                string[] result = signInHandler.Login(user.Name, model.Input.Password);

                if (result == null)
                {
                    string errorMessage = "Either the name or password is incorrect!";

                    return RedirectToAction(nameof(Login), new { errorMessage, returnUrl = model.ReturnUrl });
                }

                Trace.WriteLine("New user '" + model.Input.Name + "' has registered in succesfully!");
                model.ReturnUrl = model.ReturnUrl == null ? "/UserHome/Index" : model.ReturnUrl;
                return Redirect(model.ReturnUrl + "?Code=" + result[0] + "&UserId=" + result[1]);
            }
        }

        [HttpPost("Validate")]
        public IActionResult ValidateAccesstoken([FromBody] TokensObject tokens)
        {
            if (!String.IsNullOrEmpty(tokens.AccessToken))
            {
                return Ok(new { valid = signInHandler.ValidateAccessToken(tokens.AccessToken) });
            }
            else return StatusCode(404);
        }

        [HttpPost("Refresh")]
        public IActionResult RefreshAccesstoken([FromBody] TokensObject tokens)
        {
            if (!String.IsNullOrEmpty(tokens.RefreshToken))
            {
                return Ok(new { access_token = signInHandler.RefreshAccessToken(tokens.RefreshToken) });
            }
            else return StatusCode(404);
        }

        [HttpGet("token")]
        public IActionResult GetTokens([FromBody] TokensObject tokens)
        {
            if (!String.IsNullOrEmpty(tokens.UserId) &&
                !String.IsNullOrEmpty(tokens.Code))
            {
                string[] authTokens = signInHandler.RequestTokens(tokens.UserId, tokens.Code);

                return Ok(new { access_token = authTokens[1], refresh_token = authTokens[0] });
            }
            else return StatusCode(404);
        }

        [HttpGet("Me")]
        public IActionResult GetUserByAccesstoken([FromBody] TokensObject tokens)
        {
            if (!String.IsNullOrEmpty(tokens.AccessToken))
            {
                return Ok(signInHandler.GetUserByAccessToken(tokens.AccessToken));
            }
            else return StatusCode(404);
        }

        public class TokensObject
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }

            public string Code { get; set; }
            public string UserId { get; set; }
        }
    }
}
