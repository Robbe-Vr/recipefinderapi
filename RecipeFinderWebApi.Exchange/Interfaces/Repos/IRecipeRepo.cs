using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IRecipeRepo : IBaseEntityRepo<Recipe>
    {
        public IEnumerable<Recipe> GetAllByCook(string userId);

        public Recipe GetById(string id);
        public Recipe GetByName(string name);
    }
}
