using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class IngredientUnitTypeRelation
    {
        public int CountId { get; set; }

        public string IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int UnitTypeId { get; set; }
        public UnitType UnitType { get; set; }

        public bool Deleted { get; set; }
    }
}
