using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.Models
{
    public class ManagementSettings
    {
        public string Token { get; set; }

        public bool LoadExternalDatabases { get; set; }
    }
}
