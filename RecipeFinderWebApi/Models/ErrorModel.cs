using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.Models
{
    public class ErrorModel
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
