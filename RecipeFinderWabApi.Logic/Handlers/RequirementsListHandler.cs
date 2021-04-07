using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
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

        public RequirementsList GetByRecipeId(string id)
        {
            return _repo.GetByRecipeId(id);
        }

        public RequirementsList GetByRecipeName(string name)
        {
            return _repo.GetByRecipeName(name);
        }

        public int Create(RequirementsListIngredient ingredient)
        {
            return _repo.Create(ingredient);
        }

        public int Update(RequirementsListIngredient ingredient)
        {
            return _repo.Update(ingredient);
        }

        public int Delete(RequirementsListIngredient ingredient)
        {
            return _repo.Delete(ingredient);
        }
    }
}
