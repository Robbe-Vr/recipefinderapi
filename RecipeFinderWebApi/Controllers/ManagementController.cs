using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RecipeFinderWebApi.DAL.Mergers;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.UI.Filters;
using RecipeFinderWebApi.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.Controllers
{
    [AllowAnonymous]
    [Route("[controller]/{action}")]
    public class ManagementController : Controller
    {
        private RoleHandler _roleHandler;
        private UserHandler _userHandler;
        public ManagementController(RoleHandler roleHandler, UserHandler userHandler)
        {
            _roleHandler = roleHandler;
            _userHandler = userHandler;
        }

        public IActionResult Index(string Token)
        {
            return View(new ManagementSettings()
            {
                Token = Token,
                LoadExternalDatabases = ExternalDatabaseSettings.LoadExternalDatabases,
            });
        }

        public IActionResult ReturnUrl(string Token)
        {
            RequiresRolesFilter filter = new RequiresRolesFilter(new string[] { "Admin" }, true, _roleHandler, _userHandler);

            AuthorizationFilterContext filterContext = new AuthorizationFilterContext(
                new ActionContext(HttpContext, RouteData, new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
                new List<IFilterMetadata>())
            { HttpContext = HttpContext, Result = null };

            HttpContext.Request.Headers.Add("RecipeFinder_AccessToken", Token);

            filter.OnAuthorization(filterContext);

            if (filterContext.Result != null)
            {
                return filterContext.Result;
            }

            return RedirectToAction(nameof(Index), new { Token });
        }

        [HttpPost]
        [RequiresRoles(true, "Admin")]
        public IActionResult SetManagementSettings(ManagementSettings settings)
        {
            ExternalDatabaseSettings.LoadExternalDatabases = settings.LoadExternalDatabases;

            return RedirectToAction(nameof(Index), new { Token = settings.Token });
        }
    }
}
