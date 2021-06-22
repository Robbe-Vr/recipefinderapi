using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
{
    public class IngredientCategoryHandler
    {
        private IIngredientCategoryRepo _repo;

        private IIngredientCategoryRelationRepo _ingredient_relation_repo;

        public IngredientCategoryHandler(IIngredientCategoryRepo repo, IIngredientCategoryRelationRepo ingredient_relation_repo)
        {
            _repo = repo;

            _ingredient_relation_repo = ingredient_relation_repo;
        }

        public IEnumerable<IngredientCategory> GetAll()
        {
            return _repo.GetAll();
        }

        public IngredientCategory GetById(int id)
        {
            return _repo.GetById(id);
        }

        public IngredientCategory GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(IngredientCategory category)
        {
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

        public int Update(IngredientCategory category)
        {
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

        public int Delete(IngredientCategory category)
        {
            return _repo.Delete(GetById(category.CountId));
        }

        public int CreateIngredientRelation(Ingredient ingredient, IngredientCategory category)
        {
            return _ingredient_relation_repo.CreateRelation(ingredient, category);
        }

        public int DeleteIngredientRelation(IngredientCategoryRelation relation)
        {
            return _ingredient_relation_repo.DeleteRelation(relation);
        }

        public int DeleteIngredientRelation(Ingredient ingredient, IngredientCategory category)
        {
            var relation = _ingredient_relation_repo.GetByIngredientIdAndCategoryId(ingredient.Id, category.CountId);

            return _ingredient_relation_repo.DeleteRelation(relation);
        }
    }
}
