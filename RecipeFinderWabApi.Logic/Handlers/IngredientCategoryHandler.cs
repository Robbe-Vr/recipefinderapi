using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
{
    public class IngredientCategoryHandler
    {
        private IIngredientCategoryRepo _repo;

        public IngredientCategoryHandler(IIngredientCategoryRepo repo)
        {
            _repo = repo;
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

        public int Create(IngredientCategory ingredient)
        {
            return _repo.Create(ingredient);
        }

        public int Update(IngredientCategory ingredient)
        {
            return _repo.Update(ingredient);
        }

        public int Delete(IngredientCategory ingredient)
        {
            return _repo.Delete(ingredient);
        }
    }
}
