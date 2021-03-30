using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class IngredientCategoryRelation
    {
        public int CountId { get; set; }

        public string IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int CategoryId { get; set; }
        public IngredientCategory Category { get; set; }


        public bool Deleted { get; set; }
    }
}
