using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
{
    public class RequirementsListHandler
    {
        private IRequirementsListRepo _repo;

        public RequirementsListHandler(IRequirementsListRepo repo)
        {
            _repo = repo;
        }

        public IEnumerable<RequirementsListIngredient> GetAll()
        {
            return _repo.GetAll();
        }

        public RequirementsListIngredient GetById(int id)
        {
            return _repo.GetById(id);
        }

        public IEnumerable<RequirementsListIngredient> GetByRecipeId(string id)
        {
            return _repo.GetByRecipeId(id);
        }

        public IEnumerable<RequirementsListIngredient> GetByRecipeName(string name)
        {
            return _repo.GetByRecipeName(name);
        }

        public int Create(RequirementsListIngredient ingredient)
        {
            if (!Validate(ingredient))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(ingredient);

            if (validationResult != 0)
            {
                if (validationResult == -2)
                {
                    if (!_repo.TryRestore(ingredient))
                    {
                        return 0;
                    }
                }

                return validationResult;
            }

            return _repo.Create(ingredient);
        }

        public int Update(RequirementsListIngredient ingredient)
        {
            if (!Validate(ingredient))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(ingredient);

            if (validationResult != 0)
            {
                if (validationResult != -2)
                {
                    return validationResult;
                }
                else if (_repo.TryRestore(ingredient))
                {
                    return validationResult;
                }
            }

            return _repo.Update(ingredient);
        }

        public int Delete(RequirementsListIngredient ingredient)
        {
            return _repo.Delete(GetById(ingredient.CountId));
        }

        private bool Validate(RequirementsListIngredient ingredient)
        {
            return (ingredient.IngredientId?.Length > 2 && ingredient.RecipeId?.Length > 2 && ingredient.Units > 0.00 && ingredient.UnitTypeId > 0);
        }
    }
}
