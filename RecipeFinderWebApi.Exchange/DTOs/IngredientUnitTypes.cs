using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class IngredientUnitTypes
    {
        public string IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public ICollection<UnitType> UnitTypes { get; set; }

        public bool Deleted { get; set; }
    }
}
