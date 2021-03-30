using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
{
    public class RecipeCategoryHandler
    {
        private IRecipeCategoryRepo _repo;

        public RecipeCategoryHandler(IRecipeCategoryRepo repo)
        {
            _repo = repo;
        }

        public IEnumerable<RecipeCategory> GetAll()
        {
            return _repo.GetAll();
        }

        public RecipeCategory GetById(int id)
        {
            return _repo.GetById(id);
        }

        public RecipeCategory GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(RecipeCategory ingredient)
        {
            return _repo.Create(ingredient);
        }

        public int Update(RecipeCategory ingredient)
        {
            return _repo.Update(ingredient);
        }

        public int Delete(RecipeCategory ingredient)
        {
            return _repo.Delete(ingredient);
        }
    }
}
