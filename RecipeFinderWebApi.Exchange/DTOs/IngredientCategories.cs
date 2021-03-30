using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class IngredientCategories
    {
        public string IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public ICollection<IngredientCategory> Categories { get; set; }

        public bool Deleted { get; set; }
    }
}
