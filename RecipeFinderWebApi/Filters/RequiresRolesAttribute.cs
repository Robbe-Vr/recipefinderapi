using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.UI.Auth;
using RecipeFinderWebApi.UI.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Method)]
    public class RequiresRolesAttribute : TypeFilterAttribute
    {
        public RequiresRolesAttribute(bool matchAll = false, params string[] roleNames) : base(typeof(RequiresRolesFilter))
        {
            Arguments = new object[] { roleNames, matchAll };
        }
    }

    public class RequiresRolesFilter : IAuthorizationFilter
    {
        private RoleHandler roleHandler;
        private UserHandler userHandler;

        private List<int> requiredRoles;
        private bool matchAll;

        public RequiresRolesFilter(string[] roleNames, bool matchAll, RoleHandler roleHandler, UserHandler userHandler)
        {
            this.roleHandler = roleHandler;
            this.userHandler = userHandler;

            requiredRoles = new List<int>();
            foreach (string roleName in roleNames)
            {
                requiredRoles.Add(roleHandler.GetByName(roleName.Trim()).CountId);
            }

            this.matchAll = matchAll;

            if (requiredRoles.Count != roleNames.Length)
            {
                throw new ArgumentException("Invalid role supplied!");
            }
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string accessToken = context.HttpContext.Request.Headers["RecipeFinder_AccessToken"];

            if (String.IsNullOrEmpty(accessToken))
            {
                Trace.WriteLine($"Denied request from {context.HttpContext.Request.Host}. Reason: No access_token provided!");
                context.Result = new ContentResult
                {
                    ContentType = "text/html",
                    Content = "<html><body>No access_token provided!\n<a href=\"https://recipefinderapi.sywapps.com/api/authorize/Login" + "\">Login here</a></body></html>",
                };
                return;
            }

            User user = AuthManager.GetUser(accessToken);

            if (user == null)
            {
                Trace.WriteLine($"Denied request from {context.HttpContext.Request.Host}. Reason: Unknown access_token '{accessToken}'!");
                context.Result = new ContentResult
                {
                    ContentType = "text/html",
                    Content = "<html><body>Unknown access_token!\n<a href=\"https://recipefinderapi.sywapps.com/api/authorize/Login" + "\">Login here</a></body></html>",
                };
                return;
            }

            if (!UserIsAuthorized(user.Roles.ToArray()))
            {
                Trace.WriteLine($"Denied request from {context.HttpContext.Request.Host}. Reason: The user associated with the access_token '{accessToken}' is not authorized for this endpoint!");
                context.Result = new ContentResult
                {
                    ContentType = "text/html",
                    Content = "<html><body>The user associated with this access_token is not authorized for this endpoint!\n<a href=\"https://recipefinderapi.sywapps.com/api/authorize/Login" + "\">Login here</a></body></html>",
                };
                return;
            }
        }

        private bool UserIsAuthorized(Role[] roles)
        {
            return matchAll ? UserHasAllRequiredRoles(roles) : UserHasOneOfRequiredRoles(roles);
        }

        private bool UserHasAllRequiredRoles(Role[] roles)
        {
            foreach (int roleId in requiredRoles)
            {
                if (!roles.Any(r => r.CountId == roleId))
                {
                    return false;
                }
            }

            return true;
        }

        private bool UserHasOneOfRequiredRoles(Role[] roles)
        {
            foreach (int roleId in requiredRoles)
            {
                foreach (Role userRole in roles)
                {
                    if (roleId == userRole.CountId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
