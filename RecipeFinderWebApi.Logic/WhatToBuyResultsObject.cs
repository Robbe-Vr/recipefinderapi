using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Logic
{
    public class WhatToBuyResultsObject
    {
        public IEnumerable<WhatToBuyForRecipe> Recipes { get; set; }
        public IEnumerable<KitchenIngredient> GlobbalToBuyIngredients { get; set; }
    }

    public class WhatToBuyForRecipe
    {
        public Recipe Recipe { get; set; }
        public IEnumerable<KitchenIngredient> ToBuyIngredients { get; set; }
    }
}
