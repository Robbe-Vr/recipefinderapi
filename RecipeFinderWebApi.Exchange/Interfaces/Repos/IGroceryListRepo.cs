using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IGroceryListRepo
    {
        public IEnumerable<GroceryList> GetAll();

        public GroceryList GetById(string id);
        public GroceryList GetByName(string name);

        public int Create(GroceryList ingredient);

        public int Update(GroceryList ingredient);

        public int Delete(GroceryList ingredient);
    }
}
