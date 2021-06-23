using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
{
    public class KitchenHandler
    {
        private IKitchenRepo _repo;
        private IIngredientRepo _ingredient_repo;
        private UnitTypeHandler _unitTypeHandler;

        public KitchenHandler(IKitchenRepo repo, IIngredientRepo ingredient_repo, UnitTypeHandler unitTypeHandler)
        {
            _repo = repo;
            _ingredient_repo = ingredient_repo;
            _unitTypeHandler = unitTypeHandler;
        }

        public IEnumerable<KitchenIngredient> GetAll()
        {
            return _repo.GetAll();
        }

        public KitchenIngredient GetById(int id)
        {
            return _repo.GetById(id);
        }

        public Kitchen GetByUserId(string id)
        {
            return _repo.GetByUserId(id);
        }

        public Kitchen GetByUserName(string name)
        {
            return _repo.GetByUserName(name);
        }

        public int Create(KitchenIngredient ingredient)
        {
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
                else if (validationResult != -1)
                {
                    return validationResult;
                }
            }

            KitchenIngredient existing = GetByUserId(ingredient.UserId).Ingredients.FirstOrDefault(x => x.IngredientId == ingredient.IngredientId);
            if (existing == null)
            {
                return _repo.Create(ingredient);
            }
            else
            {
                if (existing.UnitTypeId == ingredient.UnitTypeId)
                {
                    existing.Units = ingredient.Units + (validationResult == -2 || validationResult == -1 ? existing.Units : 0);
                }
                else
                {
                    AlgorithmHelper helper = new AlgorithmHelper(_unitTypeHandler, _ingredient_repo);

                    double convertedUnits = helper.Convert(ingredient, existing.UnitType);

                    existing.Units = convertedUnits + (validationResult == -2 || validationResult == -1 ? existing.Units : 0);
                }

                return Update(existing);
            }
        }

        public int Update(KitchenIngredient ingredient)
        {
            int validationResult = _repo.ValidateOriginality(ingredient);

            if (validationResult != 0)
            {
                if (validationResult != -2 && validationResult != -1)
                {
                    return validationResult;
                }
                else if (_repo.TryRestore(ingredient))
                {
                    return validationResult;
                }
            }

            if (ingredient.CountId < 1)
            {
                var actual = GetByUserId(ingredient.UserId).Ingredients.FirstOrDefault(x => x.IngredientId == ingredient.IngredientId);
                if (actual == null) return 0;

                actual.Units = ingredient.Units;
                actual.UnitTypeId = ingredient.UnitTypeId;

                ingredient = actual;
            }
            else
            {
                var actual = GetById(ingredient.CountId);
                if (actual == null) return 0;

                actual.Units = ingredient.Units;
                actual.UnitTypeId = ingredient.UnitTypeId;

                ingredient = actual;
            }

            return _repo.Update(ingredient);
        }

        public int Delete(KitchenIngredient ingredient)
        {
            if (ingredient.CountId < 1)
            {
                var actual = GetByUserId(ingredient.UserId).Ingredients.FirstOrDefault(x => x.IngredientId == ingredient.IngredientId);
                if (actual == null) return 0;

                ingredient = actual;
            }
            else
            {
                var actual = GetById(ingredient.CountId);
                if (actual == null) return 0;

                ingredient = actual;
            }

            return _repo.Delete(ingredient);
        }
    }
}
