using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IIngredientRepo
    {
        public IEnumerable<Ingredient> GetAll();

        public Ingredient GetById(string id);
        public Ingredient GetByName(string name);

        public int Create(Ingredient ingredient);

        public int Update(Ingredient ingredient);

        public int Delete(Ingredient ingredient);
    }
}
