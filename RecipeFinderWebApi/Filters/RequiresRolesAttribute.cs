using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.UI.Auth;
using System;
using System.Collections.Generic;
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
                context.Result = new ObjectResult("No access_token provided!") { StatusCode = 401 };
                return;
            }

            User user = AuthManager.GetUser(accessToken);

            if (user == null)
            {
                context.Result = new ObjectResult("Unknown access_token!") { StatusCode = 401 };
                return;
            }

            if (!UserIsAuthorized(user.Roles.ToArray()))
            {
                context.Result = new ObjectResult("The user associated with this access_token is not authorized for this endpoint!!") { StatusCode = 401 };
                return;
            }

        }

        private bool UserIsAuthorized(Role[] roles)
        {
            return matchAll ? UserHasRequiredRoles(roles) : UserHasRequiredRole(roles);
        }

        private bool UserHasRequiredRoles(Role[] roles)
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

        private bool UserHasRequiredRole(Role[] roles)
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
