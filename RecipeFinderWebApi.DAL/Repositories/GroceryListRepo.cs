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
            list.User = null;

            if (!Exists(list))
            {
                return 0;
            }
            if (!EntityIsAttached(list))
            {
                if (KeyIsAttached(list))
                {
                    GroceryList old = GetAttachedEntityByEntity(list);

                    old.Name = list.Name;
                    old.Value = list.Value;
                    old.Deleted = list.Deleted;
                }
                else context.GroceryLists.Update(list);
            }

            return context.SaveChanges();
        }

        public int Delete(GroceryList list)
        {
            list.User = null;

            if (!Exists(list))
            {
                return 0;
            }
            if (!EntityIsAttached(list))
            {
                if (KeyIsAttached(list))
                {
                    list = GetAttachedEntityByEntity(list);
                }
                else context.GroceryLists.Update(list);
            }

            list.Deleted = true;

            return context.SaveChanges();
        }
    }
}
