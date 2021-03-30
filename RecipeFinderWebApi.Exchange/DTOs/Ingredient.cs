using System;
using System.Collections.Generic;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class Ingredient
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<IngredientCategory> Categories { get; set; }
        public ICollection<UnitType> UnitTypes { get; set; }

        public bool Deleted { get; set; }
    }
}
