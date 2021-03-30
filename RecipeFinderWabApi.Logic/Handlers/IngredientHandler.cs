using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
{
    public class IngredientHandler
    {
        private IIngredientRepo _repo;

        public IngredientHandler(IIngredientRepo repo)
        {
            _repo = repo;
        }

        public IEnumerable<Ingredient> GetAll()
        {
            return _repo.GetAll();
        }

        public Ingredient GetById(string id)
        {
            return _repo.GetById(id);
        }

        public Ingredient GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(Ingredient ingredient)
        {
            return _repo.Create(ingredient);
        }

        public int Update(Ingredient ingredient)
        {
            return _repo.Update(ingredient);
        }

        public int Delete(Ingredient ingredient)
        {
            return _repo.Delete(ingredient);
        }
    }
}
