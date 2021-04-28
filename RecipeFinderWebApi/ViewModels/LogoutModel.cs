using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.ViewModels
{
    public class LogoutModel
    {
        public string LogoutId { get; set; }
        public bool TriggerExternalSignout { get; set; }
        public string ExternalAuthenticationScheme { get; set; }
    }
}
