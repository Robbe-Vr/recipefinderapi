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

        public KitchenHandler(IKitchenRepo repo)
        {
            _repo = repo;
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
            KitchenIngredient existing = GetByUserId(ingredient.UserId).Ingredients.FirstOrDefault(x => x.IngredientId == ingredient.IngredientId);
            if (existing == null)
            {
                return _repo.Create(ingredient);
            }
            else
            {
                if (existing.UnitTypeId == ingredient.UnitTypeId)
                {
                    existing.Units += ingredient.Units;
                }
                else
                {
                    existing.UnitTypeId = ingredient.UnitTypeId;
                    existing.Units = ingredient.Units;
                }

                return Update(existing);
            }
        }

        public int Update(KitchenIngredient ingredient)
        {
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
