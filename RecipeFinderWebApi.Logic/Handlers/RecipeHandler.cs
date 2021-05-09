using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
{
    public class RecipeHandler
    {
        private IRecipeRepo _repo;

        private IRecipeCategoryRelationRepo _category_relation_repo;

        private IRequirementsListRepo _requirementsList_repo;

        public RecipeHandler(IRecipeRepo repo, IRecipeCategoryRelationRepo category_relation_repo, IRequirementsListRepo requirementsList_repo)
        {
            _repo = repo;

            _category_relation_repo = category_relation_repo;

            _requirementsList_repo = requirementsList_repo;
        }

        public IEnumerable<RecipeWithRequirements> GetAll()
        {
            IEnumerable<Recipe> recipes = _repo.GetAll();

            List<RecipeWithRequirements> recipesWithRequirements = new List<RecipeWithRequirements>();

            foreach (Recipe recipe in recipes)
            {
                RecipeWithRequirements recipeWithRequirements = new RecipeWithRequirements(recipe);

                IEnumerable<RequirementsListIngredient> ingredients = _requirementsList_repo.GetByRecipeId(recipe.Id);

                recipeWithRequirements.RequirementsList = new RequirementsList()
                {
                    Ingredients = ingredients.ToList(),
                    RecipeId = recipe.Id,
                    Recipe = recipe,
                };

                recipesWithRequirements.Add(recipeWithRequirements);
            }

            return recipesWithRequirements;
        }

        public RecipeWithRequirements GetById(string id)
        {
            RecipeWithRequirements recipe = new RecipeWithRequirements(_repo.GetById(id));

            if (recipe != null)
            {
                List<RequirementsListIngredient> ingredients = _requirementsList_repo.GetByRecipeId(recipe.Id).ToList();

                recipe.RequirementsList = new RequirementsList()
                {
                    Ingredients = ingredients,
                    RecipeId = recipe.Id,
                    Recipe = recipe,
                };
            }

            return recipe;
        }

        public RecipeWithRequirements GetByName(string name)
        {
            RecipeWithRequirements recipe = (RecipeWithRequirements)_repo.GetByName(name);

            if (recipe != null)
            {
                List<RequirementsListIngredient> ingredients = _requirementsList_repo.GetByRecipeId(recipe.Id).ToList();

                recipe.RequirementsList = new RequirementsList()
                {
                    Ingredients = ingredients,
                    RecipeId = recipe.Id,
                    Recipe = recipe,
                };
            }

            return recipe;
        }

        public int Create(RecipeWithRequirements recipe)
        {
            int changes = 0;

            RequirementsList requirementsList = new RequirementsList();
            requirementsList.Ingredients = recipe.RequirementsList?.Ingredients;

            List<RecipeCategory> Categories = new List<RecipeCategory>();
            Categories.AddRange(recipe.Categories);

            changes += _repo.Create(recipe);

            recipe = GetByName(recipe.Name);

            if (Categories.Count > 0)
            {
                foreach (RecipeCategory category in Categories)
                {
                    changes += CreateCategoryRelation(recipe, category);
                }
            }
            
            if (requirementsList.Ingredients.Count > 0)
            {
                foreach(RequirementsListIngredient ingredient in requirementsList.Ingredients)
                {
                    ingredient.RecipeId = recipe.Id;

                    changes += _requirementsList_repo.Create(ingredient);
                }
            }

            return changes;
        }

        public int Update(RecipeWithRequirements recipe)
        {
            int changes = 0;

            var currentState = GetById(recipe.Id);

            recipe.CountId = currentState.CountId;

            List<RecipeCategory> Categories = new List<RecipeCategory>();
            Categories.AddRange(recipe.Categories);

            RequirementsList requirementsList = new RequirementsList();
            requirementsList.Ingredients = recipe.RequirementsList?.Ingredients;

            changes += _repo.Update(recipe);

            if (Categories.Count > 0)
            {
                IEnumerable<RecipeCategory> toAddCategories = Categories.Where(x => !currentState.Categories.Select(x => x.CountId).Contains(x.CountId));

                foreach (RecipeCategory category in toAddCategories)
                {
                    changes += CreateCategoryRelation(recipe, category);
                }

                IEnumerable<RecipeCategory> toRemoveCategories = currentState.Categories.Where(x => !Categories.Select(x => x.CountId).Contains(x.CountId));

                foreach (RecipeCategory category in toRemoveCategories)
                {
                    changes += DeleteCategoryRelation(recipe, category);
                }
            }

            if (requirementsList.Ingredients.Count > 0)
            {
                IEnumerable<RequirementsListIngredient> toAddIngredients = requirementsList.Ingredients.Where(x => !currentState.RequirementsList.Ingredients.Select(x => x.IngredientId).Contains(x.IngredientId));

                foreach (RequirementsListIngredient ingredient in toAddIngredients)
                {
                    ingredient.RecipeId = currentState.Id;

                    changes += _requirementsList_repo.Create(ingredient);
                }

                IEnumerable<RequirementsListIngredient> toRemoveIngredients = currentState.RequirementsList.Ingredients.Where(x => !requirementsList.Ingredients.Select(x => x.IngredientId).Contains(x.IngredientId));

                foreach (RequirementsListIngredient ingredient in toRemoveIngredients)
                {
                    ingredient.RecipeId = currentState.Id;

                    changes += _requirementsList_repo.Delete(ingredient);
                }

                IEnumerable<RequirementsListIngredient> toUpdateIngredients = requirementsList.Ingredients.Where(x => currentState.RequirementsList.Ingredients.Any(y => x.IngredientId == y.IngredientId && (x.Units != y.Units || x.UnitTypeId != y.UnitTypeId)));

                foreach (RequirementsListIngredient ingredient in toUpdateIngredients)
                {
                    ingredient.RecipeId = currentState.Id;

                    ingredient.CountId = currentState.RequirementsList.Ingredients.FirstOrDefault(i => i.IngredientId == ingredient.IngredientId).CountId;

                    changes += _requirementsList_repo.Update(ingredient);
                }
            }

            return changes;
        }

        public int Delete(RecipeWithRequirements recipe)
        {
            int changes = 0;

            var currentState = GetById(recipe.Id);

            List<RecipeCategory> Categories = new List<RecipeCategory>();
            Categories.AddRange(currentState.Categories);

            RequirementsList requirementsList = new RequirementsList();
            requirementsList.Ingredients = recipe.RequirementsList?.Ingredients;

            changes += _repo.Delete(currentState);

            foreach (RecipeCategory category in Categories)
            {
                changes += DeleteCategoryRelation(recipe, category);
            }

            if (requirementsList != null && requirementsList.Ingredients != null)
            {
                foreach (RequirementsListIngredient ingredient in requirementsList.Ingredients)
                {
                    changes += _requirementsList_repo.Delete(ingredient);
                }
            }

            return changes;
        }

        public int CreateCategoryRelation(Recipe recipe, RecipeCategory category)
        {
            return _category_relation_repo.CreateRelation(recipe, category);
        }

        public int DeleteCategoryRelation(RecipeCategoryRelation relation)
        {
            return _category_relation_repo.DeleteRelation(relation);
        }

        public int DeleteCategoryRelation(Recipe recipe, RecipeCategory category)
        {
            var relation = _category_relation_repo.GetByRecipeIdAndCategoryId(recipe.Id, category.CountId);

            return _category_relation_repo.DeleteRelation(relation);
        }

    }
}
