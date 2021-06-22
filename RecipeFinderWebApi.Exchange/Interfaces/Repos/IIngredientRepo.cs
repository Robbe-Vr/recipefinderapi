using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IIngredientRepo : IBaseEntityRepo<Ingredient>
    {
        public Ingredient GetById(string id);
        public Ingredient GetByName(string name);

        public Ingredient CreateGetId(Ingredient ingredient);
    }
}
