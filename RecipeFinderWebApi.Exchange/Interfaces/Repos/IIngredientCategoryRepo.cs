using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IIngredientCategoryRepo : IBaseEntityRepo<IngredientCategory>
    {
        public IngredientCategory GetByName(string name);
    }
}
