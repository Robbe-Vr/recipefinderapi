using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IRecipeRepo
    {
        public IEnumerable<Recipe> GetAll();

        public Recipe GetById(string id);
        public Recipe GetByName(string name);

        public int Create(Recipe recipe);

        public int Update(Recipe recipe);

        public int Delete(Recipe recipe);
    }
}
