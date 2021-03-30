using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class GroceryListRepo : IGroceryListRepo
    {
        private RecipeFinderDBContext context = new RecipeFinderDBContext(RecipeFinderDBContext.ops.dbOptions);

        public IEnumerable<GroceryList> GetAll()
        {
            return context.GroceryLists
                .Include(x => x.User)
                .Where(x => !x.Deleted);
        }

        public GroceryList GetById(string id)
        {
            return context.GroceryLists
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public GroceryList GetByName(string name)
        {
            return context.GroceryLists
                .Include(x => x.User)
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public int Create(GroceryList ingredient)
        {
            context.GroceryLists.Add(ingredient);

            return context.SaveChanges();
        }

        public int Update(GroceryList ingredient)
        {
            context.GroceryLists.Update(ingredient);

            return context.SaveChanges();
        }

        public int Delete(GroceryList ingredient)
        {
            ingredient.Deleted = true;

            context.GroceryLists.Update(ingredient);

            return context.SaveChanges();
        }
    }
}
