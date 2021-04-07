using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
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

        public IEnumerable<Recipe> GetAll()
        {
            return _repo.GetAll();
        }

        public Recipe GetById(string id)
        {
            return _repo.GetById(id);
        }

        public Recipe GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(Recipe recipe)
        {
            int changes = 0;

            RequirementsList requirementsList = new RequirementsList();
            requirementsList.Ingredients = recipe.RequirementsList?.Ingredients;

            changes += _repo.Create(recipe);

            if (recipe.Categories.Count > 0)
            {
                foreach (RecipeCategory category in recipe.Categories)
                {
                    changes += CreateCategoryRelation(recipe, category);
                }
            }
            
            if (requirementsList.Ingredients.Count > 0)
            {
                foreach(RequirementsListIngredient ingredient in requirementsList.Ingredients)
                {
                    changes += _requirementsList_repo.Create(ingredient);
                }
            }

            return changes;
        }

        public int Update(Recipe recipe)
        {
            int changes = 0;

            var currentState = GetById(recipe.Id);

            recipe.CountId = currentState.CountId;

            RequirementsList requirementsList = new RequirementsList();
            requirementsList.Ingredients = recipe.RequirementsList?.Ingredients;

            changes += _repo.Update(recipe);

            if (recipe.Categories.Count > 0)
            {
                IEnumerable<RecipeCategory> toAddCategories = recipe.Categories.Where(x => !currentState.Categories.Select(x => x.CountId).Contains(x.CountId));

                foreach (RecipeCategory category in toAddCategories)
                {
                    changes += CreateCategoryRelation(recipe, category);
                }

                IEnumerable<RecipeCategory> toRemoveCategories = currentState.Categories.Where(x => !recipe.Categories.Select(x => x.CountId).Contains(x.CountId));

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
                    changes += _requirementsList_repo.Create(ingredient);
                }

                IEnumerable<RequirementsListIngredient> toRemoveIngredients = currentState.RequirementsList.Ingredients.Where(x => !requirementsList.Ingredients.Select(x => x.IngredientId).Contains(x.IngredientId));

                foreach (RequirementsListIngredient ingredient in toRemoveIngredients)
                {
                    changes += _requirementsList_repo.Delete(ingredient);
                }

                IEnumerable<RequirementsListIngredient> toUpdateIngredients = requirementsList.Ingredients.Where(x => currentState.RequirementsList.Ingredients.Any(y => x.IngredientId == y.IngredientId && x.Units == y.Units && x.UnitTypeId == y.UnitTypeId));

                foreach (RequirementsListIngredient ingredient in toUpdateIngredients)
                {
                    changes += _requirementsList_repo.Update(ingredient);
                }
            }

            return changes;
        }

        public int Delete(Recipe recipe)
        {
            int changes = 0;

            var currentState = GetById(recipe.Id);

            changes += _repo.Delete(currentState);

            foreach (RecipeCategory category in currentState.Categories)
            {
                changes += DeleteCategoryRelation(recipe, category);
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
