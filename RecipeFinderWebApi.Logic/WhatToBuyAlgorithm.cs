using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using RecipeFinderWebApi.Logic.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.Logic
{
    public class WhatToBuyAlgorithm
    {
        private RecipeHandler _recipeHandler;
        private KitchenHandler _kitchenHandler;

        private AlgorithmHelper _helper;

        public WhatToBuyAlgorithm(RecipeHandler recipeHandler, KitchenHandler kitchenHandler, UnitTypeHandler unitTypeHandler)
        {
            _recipeHandler = recipeHandler;
            _kitchenHandler = kitchenHandler;
            _helper = new AlgorithmHelper(unitTypeHandler);
        }

        public IEnumerable<RecipeWithRequirements> GetWhatToBuyForUser(string userId)
        {
            Kitchen userKitchen = _kitchenHandler.GetByUserId(userId);

            var preparableRecipes = FindMatchingRecipes(userKitchen.Ingredients);

            return preparableRecipes;
        }

        private IEnumerable<RecipeWithRequirements> FindMatchingRecipes(IEnumerable<KitchenIngredient> kitchenIngredients)
        {
            List<RecipeWithRequirements> unmatchingRecipes = new List<RecipeWithRequirements>();

            IEnumerable<RecipeWithRequirements> allRecipes = _recipeHandler.GetAll();

            foreach (KitchenIngredient ingredient in kitchenIngredients)
            {
                List<RecipeWithRequirements> recipesWithIngredients = allRecipes.Where(r => r.RequirementsList.Ingredients.Any(x => x.IngredientId == ingredient.IngredientId)).ToList();

                foreach (RecipeWithRequirements recipe in recipesWithIngredients)
                {
                    RequirementsListIngredient required = recipe.RequirementsList.Ingredients.First(x => x.IngredientId == ingredient.IngredientId);

                    KitchenIngredient present = ingredient;

                    double missingAmount = CalculateMissingAmount(present, required);

                    if (missingAmount > 0)
                    {
                        RecipeWithRequirements unmatchingRecipe = unmatchingRecipes.FirstOrDefault(r => r.CountId == recipe.CountId);

                        if (unmatchingRecipe != null)
                        {
                            unmatchingRecipe.RequirementsList.Ingredients.Add(
                                new RequirementsListIngredient()
                                {
                                    CountId = required.CountId,
                                    IngredientId = required.IngredientId,
                                    Ingredient = required.Ingredient,
                                    UnitTypeId = _helper.LastUsed.CountId,
                                    UnitType = _helper.LastUsed,
                                    RecipeId = recipe.Id,
                                    Recipe = recipe,
                                    Units = missingAmount,
                                    Deleted = required.Deleted,
                                }
                            );
                        }
                        else
                        {
                            unmatchingRecipes.Add(
                                new RecipeWithRequirements(recipe)
                                {
                                    RequirementsList = new RequirementsList()
                                    {
                                        RecipeId = recipe.Id,
                                        Recipe = recipe,
                                        Ingredients = new List<RequirementsListIngredient>()
                                        {
                                            new RequirementsListIngredient()
                                            {
                                                CountId = required.CountId,
                                                IngredientId = required.IngredientId,
                                                Ingredient = required.Ingredient,
                                                UnitTypeId = _helper.LastUsed.CountId,
                                                UnitType = _helper.LastUsed,
                                                RecipeId = recipe.Id,
                                                Recipe = recipe,
                                                Units = missingAmount,
                                                Deleted = required.Deleted,
                                            },
                                        }
                                    },
                                }
                            );

                        }
                    }
                }
            }

            return unmatchingRecipes.OrderBy(r => unmatchingRecipes.Count(x => x.CountId == r.CountId) != r.RequirementsList.Ingredients.Sum(x => x.Units));
        }

        private double CalculateMissingAmount(KitchenIngredient present, RequirementsListIngredient required)
        {
            double[] evenedOutAmounts = _helper.EvenOutUnits(present, required);

            double presentEqualAmount = evenedOutAmounts[0];
            double requiredEqualAmount = evenedOutAmounts[1];

            return requiredEqualAmount - presentEqualAmount;
        }
    }
}
