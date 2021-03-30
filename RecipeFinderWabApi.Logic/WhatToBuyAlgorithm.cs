using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWabApi.Logic
{
    public class WhatToBuyAlgorithm
    {
        private IKitchenRepo _kitchenRepo;
        private IIngredientRepo _ingredientRepo;
        private IRecipeRepo _recipeRepo;
        private IRequirementsListRepo _requirementsListRepo;

        public WhatToBuyAlgorithm(IKitchenRepo kitchenRepo, IIngredientRepo ingredientRepo, IRecipeRepo recipeRepo, IRequirementsListRepo requirementsListRepo)
        {
            _kitchenRepo = kitchenRepo;
            _ingredientRepo = ingredientRepo;
            _recipeRepo = recipeRepo;
            _requirementsListRepo = requirementsListRepo;
        }

        public IEnumerable<KitchenIngredient> Calculate(WhatToBuyFilterObject filters)
        {
            var currentKitchen = _kitchenRepo.GetById(filters.UserId);

            var toBuyIngredients = GetIngredients(currentKitchen.Ingredients, filters);

            return toBuyIngredients;
        }

        private IEnumerable<KitchenIngredient> GetIngredients(IEnumerable<KitchenIngredient> currentIngredients, WhatToBuyFilterObject filters)
        {
            return new List<KitchenIngredient>(filters.MaxResultCount).Where(x => ValidateIngredientForFilters(x, filters));
        }

        private bool ValidateIngredientForFilters(KitchenIngredient ingredient, WhatToBuyFilterObject filters)
        {
            foreach (IngredientCategory category in ingredient.Ingredient.Categories)
            {
                filters.DisallowedIngredientCategories.Contains(category);

                return false;
            }

            return true;
        }
    }
}
