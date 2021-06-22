using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IRecipeCategoryRepo : IBaseEntityRepo<RecipeCategory>
    {
        public RecipeCategory GetByName(string name);
    }
}
