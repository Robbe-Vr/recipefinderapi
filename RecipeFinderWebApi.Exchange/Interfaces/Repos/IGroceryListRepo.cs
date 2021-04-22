using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IGroceryListRepo
    {
        public IEnumerable<GroceryList> GetAll();

        public IEnumerable<GroceryList> GetAllByUserId(string id);

        public GroceryList GetById(string id);

        public GroceryList GetByName(string name);

        public int Create(GroceryList list);

        public int Update(GroceryList list);

        public int Delete(GroceryList list);
    }
}
