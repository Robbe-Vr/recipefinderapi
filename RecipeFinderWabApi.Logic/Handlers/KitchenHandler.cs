using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
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
            return _repo.Create(ingredient);
        }

        public int Update(KitchenIngredient ingredient)
        {
            return _repo.Update(ingredient);
        }

        public int Delete(KitchenIngredient ingredient)
        {
            return _repo.Delete(ingredient);
        }
    }
}
