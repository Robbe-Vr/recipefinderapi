using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IRequirementsListRepo : IBaseEntityRepo<RequirementsListIngredient>
    {
        public IEnumerable<RequirementsListIngredient> GetByRecipeId(string id);

        public IEnumerable<RequirementsListIngredient> GetByRecipeName(string name);
    }
}
