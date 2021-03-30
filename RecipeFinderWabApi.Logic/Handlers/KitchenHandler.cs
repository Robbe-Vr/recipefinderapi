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

        public Kitchen GetById(string id)
        {
            return _repo.GetById(id);
        }

        public Kitchen GetByName(string name)
        {
            return _repo.GetByName(name);
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
