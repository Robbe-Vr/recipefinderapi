using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class RecipeCategories
    {
        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public ICollection<RecipeCategory> Categories { get; set; }

        public bool Deleted { get; set; }
    }
}
