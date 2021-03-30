using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic
{
    public class WhatToBuyFilterObject
    {
        public string UserId { get; set; }

        public List<IngredientCategory> DisallowedIngredientCategories { get; set; }
        public List<Ingredient> DisallowedIngredients { get; set; }

        public List<RecipeCategories> DisallowedRecipeCategories { get; set; }
        public List<Recipe> DisallowedRecipes { get; set; }

        public int MaxResultCount { get; set; }
    }
}
