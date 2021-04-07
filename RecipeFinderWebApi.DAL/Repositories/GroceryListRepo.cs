using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class GroceryListRepo : AbstractRepo<GroceryList>, IGroceryListRepo
    {
        public GroceryListRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<GroceryList> GetAll()
        {
            return context.GroceryLists
                .Include(x => x.User)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public IEnumerable<GroceryList> GetAllByUserId(string id)
        {
            return context.GroceryLists
                .Include(x => x.User)
                .AsNoTracking()
                .Where(x => x.UserId == id && !x.Deleted);
        }

        public GroceryList GetById(string id)
        {
            return context.GroceryLists
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public GroceryList GetByName(string name)
        {
            return context.GroceryLists
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public int Create(GroceryList list)
        {
            list.User = null;

            list.Id = Guid.NewGuid().ToString();

            context.GroceryLists.Add(list);

            return context.SaveChanges();
        }

        public int Update(GroceryList list)
        {
            context.GroceryLists.Update(list);

            return context.SaveChanges();
        }

        public int Delete(GroceryList list)
        {
            list.Deleted = true;

            context.GroceryLists.Update(list);

            return context.SaveChanges();
        }
    }
}
