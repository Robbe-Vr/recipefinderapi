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

        private AlgorithmHelper _helper;

        public PreparableRecipesAlgorithm(RecipeHandler recipeHandler, KitchenHandler kitchenHandler, UnitTypeHandler unitTypeHandler)
        {
            _recipeHandler = recipeHandler;
            _kitchenHandler = kitchenHandler;
            _helper = new AlgorithmHelper(unitTypeHandler);
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
            double[] evenedOutAmounts = _helper.EvenOutUnits(present, required);

            double presentEqualAmount = evenedOutAmounts[0];
            double requiredEqualAmount = evenedOutAmounts[1];

            return presentEqualAmount >= requiredEqualAmount;
        }
    }
}
