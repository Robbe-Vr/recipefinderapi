using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
{
    public class GroceryListHandler
    {
        private IGroceryListRepo _repo;

        public GroceryListHandler(IGroceryListRepo repo)
        {
            _repo = repo;
        }

        public IEnumerable<GroceryList> GetAll()
        {
            return _repo.GetAll();
        }

        public IEnumerable<GroceryList> GetAllByUserId(string id)
        {
            return _repo.GetAllByUserId(id);
        }

        public GroceryList GetById(string id)
        {
            return _repo.GetById(id);
        }

        public GroceryList GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(GroceryList ingredient)
        {
            return _repo.Create(ingredient);
        }

        public int Update(GroceryList ingredient)
        {
            return _repo.Update(ingredient);
        }

        public int Delete(GroceryList ingredient)
        {
            return _repo.Delete(ingredient);
        }
    }
}
