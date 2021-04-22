using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RecipeFinderWabApi.Logic.Handlers;
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

        private List<int> roles;
        private bool matchAll;

        public RequiresRolesFilter(string[] roleNames, bool matchAll, RoleHandler roleHandler, UserHandler userHandler)
        {
            this.roleHandler = roleHandler;
            this.userHandler = userHandler;

            roles = new List<int>();
            foreach (string roleName in roleNames)
            {
                roles.Add(roleHandler.GetByName(roleName.Trim()).CountId);
            }

            this.matchAll = matchAll;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
        }
    }
}
