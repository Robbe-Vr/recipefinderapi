using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
{
    public class RecipeHandler
    {
        private IRecipeRepo _repo;

        public RecipeHandler(IRecipeRepo repo)
        {
            _repo = repo;
        }

        public IEnumerable<Recipe> GetAll()
        {
            return _repo.GetAll();
        }

        public Recipe GetById(string id)
        {
            return _repo.GetById(id);
        }

        public Recipe GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(Recipe recipe)
        {
            return _repo.Create(recipe);
        }

        public int Update(Recipe recipe)
        {
            return _repo.Update(recipe);
        }

        public int Delete(Recipe recipe)
        {
            return _repo.Delete(recipe);
        }
    }
}
