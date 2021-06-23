using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
{
    public class RecipeCategoryHandler
    {
        private IRecipeCategoryRepo _repo;

        private IRecipeCategoryRelationRepo _recipe_relation_repo;

        public RecipeCategoryHandler(IRecipeCategoryRepo repo, IRecipeCategoryRelationRepo recipe_relation_repo)
        {
            _repo = repo;

            _recipe_relation_repo = recipe_relation_repo;
        }

        public IEnumerable<RecipeCategory> GetAll()
        {
            return _repo.GetAll();
        }

        public RecipeCategory GetById(int id)
        {
            return _repo.GetById(id);
        }

        public RecipeCategory GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(RecipeCategory category)
        {
            if (!Validate(category))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(category);

            if (validationResult != 0)
            {
                if (validationResult == -2)
                {
                    if (!_repo.TryRestore(category))
                    {
                        return 0;
                    }
                }

                return validationResult;
            }

            return _repo.Create(category);
        }

        public int CreateRecipeRelation(Recipe recipe, RecipeCategory category)
        {
            return _recipe_relation_repo.CreateRelation(recipe, category);
        }

        public int DeleteRecipeRelation(RecipeCategoryRelation relation)
        {
            return _recipe_relation_repo.DeleteRelation(relation);
        }

        public int DeleteRecipeRelation(Recipe recipe, RecipeCategory category)
        {
            var relation = _recipe_relation_repo.GetByRecipeIdAndCategoryId(recipe.Id, category.CountId);

            return _recipe_relation_repo.DeleteRelation(relation);
        }

        public int Update(RecipeCategory category)
        {
            if (!Validate(category))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(category);

            if (validationResult != 0)
            {
                if (validationResult != -2)
                {
                    return validationResult;
                }
                else if (_repo.TryRestore(category))
                {
                    return validationResult;
                }
            }

            return _repo.Update(category);
        }

        public int Delete(RecipeCategory category)
        {
            return _repo.Delete(GetById(category.CountId));
        }

        private bool Validate(RecipeCategory category)
        {
            return (category.Name?.Length > 2);
        }
    }
}
