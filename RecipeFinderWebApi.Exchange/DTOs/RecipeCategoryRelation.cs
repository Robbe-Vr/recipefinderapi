using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class RecipeCategoryRelation
    {
        public int CountId { get; set; }

        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int CategoryId { get; set; }
        public RecipeCategory Category { get; set; }

        public bool Deleted { get; set; }
    }
}
