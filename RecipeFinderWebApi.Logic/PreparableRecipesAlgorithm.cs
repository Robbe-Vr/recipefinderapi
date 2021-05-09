using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Logic.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.Logic
{
    public class PreparableRecipesAlgorithm
    {
        private RecipeHandler _recipeHandler;
        private KitchenHandler _kitchenHandler;
        private UnitTypeHandler _unitTypeHandler;

        public PreparableRecipesAlgorithm(RecipeHandler recipeHandler, KitchenHandler kitchenHandler, UnitTypeHandler unitTypeHandler)
        {
            _recipeHandler = recipeHandler;
            _kitchenHandler = kitchenHandler;
            _unitTypeHandler = unitTypeHandler;
        }

        public IEnumerable<RecipeWithRequirements> GetPreparableForUser(string userId)
        {
            Kitchen userKitchen = _kitchenHandler.GetByUserId(userId);

            var preparableRecipes = FindMatchingRecipes(userKitchen.Ingredients);

            return preparableRecipes;
        }

        private IEnumerable<RecipeWithRequirements> FindMatchingRecipes(IEnumerable<KitchenIngredient> kitchenIngredients)
        {
            List<RecipeWithRequirements> matchingRecipes = new List<RecipeWithRequirements>();

            IEnumerable<RecipeWithRequirements> allRecipes = _recipeHandler.GetAll();

            foreach (KitchenIngredient ingredient in kitchenIngredients)
            {
                List<RecipeWithRequirements> recipesWithIngredients = allRecipes.Where(r => r.RequirementsList.Ingredients.Any(x => x.IngredientId == ingredient.IngredientId)).ToList();

                foreach (RecipeWithRequirements recipe in recipesWithIngredients)
                {
                    RequirementsListIngredient required = recipe.RequirementsList.Ingredients.First(x => x.IngredientId == ingredient.IngredientId);

                    KitchenIngredient present = ingredient;

                    if (PresentAmountMoreOrEqualToRequiredAmount(present, required))
                    {
                        matchingRecipes.Add(recipe);
                    }
                }
            }

            return matchingRecipes.Where(r => matchingRecipes.Count(x => x.CountId == r.CountId) == r.RequirementsList.Ingredients.Count).Distinct();
        }

        private bool PresentAmountMoreOrEqualToRequiredAmount(KitchenIngredient present, RequirementsListIngredient required)
        {
            double presentEqualAmount = 0.0;
            double requiredEqualAmount = 0.0;

            if (present.UnitTypeId == required.UnitTypeId)
            {
                presentEqualAmount = present.Units;
                requiredEqualAmount = required.Units;
            }
            else
            {
                if (present.Ingredient.UnitTypes.Any(x => x.CountId == _unitTypeHandler.GetByName("Units").CountId))
                {
                    if (present.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        presentEqualAmount = present.Units / present.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (present.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        presentEqualAmount = present.Units / present.Ingredient.AverageVolumeInLiterPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        presentEqualAmount = present.Units;
                    }

                    if (required.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        requiredEqualAmount = required.Units / required.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        requiredEqualAmount = required.Units / required.Ingredient.AverageVolumeInLiterPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        requiredEqualAmount = required.Units;
                    }
                }
                else if (present.Ingredient.UnitTypes.Any(x => x.CountId == _unitTypeHandler.GetByName("Kg").CountId))
                {
                    if (present.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        presentEqualAmount = present.Units;
                    }
                    else if (present.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        presentEqualAmount = (present.Units / present.Ingredient.AverageVolumeInLiterPerUnit) * present.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        presentEqualAmount = present.Units * present.Ingredient.AverageWeightInKgPerUnit;
                    }

                    if (required.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        requiredEqualAmount = required.Units;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        requiredEqualAmount = (present.Units / present.Ingredient.AverageVolumeInLiterPerUnit) * present.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        requiredEqualAmount = required.Units * present.Ingredient.AverageWeightInKgPerUnit;
                    }
                }
                else if (present.Ingredient.UnitTypes.Any(x => x.CountId == _unitTypeHandler.GetByName("L").CountId))
                {
                    if (present.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        presentEqualAmount = present.Units;
                    }
                    else if (present.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        presentEqualAmount = (present.Units / present.Ingredient.AverageVolumeInLiterPerUnit) * present.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        presentEqualAmount = present.Units * present.Ingredient.AverageVolumeInLiterPerUnit;
                    }

                    if (required.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        requiredEqualAmount = (required.Units / required.Ingredient.AverageWeightInKgPerUnit) * required.Ingredient.AverageVolumeInLiterPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        requiredEqualAmount = required.Units;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        requiredEqualAmount = required.Units * required.Ingredient.AverageVolumeInLiterPerUnit;
                    }
                }
            }

            return presentEqualAmount >= requiredEqualAmount;
        }
    }
}
