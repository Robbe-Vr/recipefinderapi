using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IGroceryListRepo : IBaseEntityRepo<GroceryList>
    {

        public IEnumerable<GroceryList> GetAllByUserId(string id);

        public GroceryList GetById(string id);

        public GroceryList GetByName(string name);
    }
}
