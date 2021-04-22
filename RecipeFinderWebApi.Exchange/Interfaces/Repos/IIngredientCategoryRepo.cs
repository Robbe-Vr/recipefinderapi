using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IIngredientCategoryRepo
    {
        public IEnumerable<IngredientCategory> GetAll();

        public IngredientCategory GetById(int id);
        public IngredientCategory GetByName(string name);

        public int Create(IngredientCategory category);

        public int Update(IngredientCategory category);

        public int Delete(IngredientCategory category);
    }
}
