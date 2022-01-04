using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class GroceryListRepo : AbstractBaseEntityRepo<GroceryList>, IGroceryListRepo
    {
        public GroceryListRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.GroceryLists))
        {
        }

        public override IEnumerable<GroceryList> GetAll()
        {
            return db
                .Include(x => x.User)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public IEnumerable<GroceryList> GetAllByUserId(string id)
        {
            return db
                .Include(x => x.User)
                .AsNoTracking()
                .Where(x => (x.UserId == id) && !x.Deleted);
        }

        public override GroceryList GetById(int id)
        {
            return db
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefault(x => (x.CountId == id) && !x.Deleted);
        }

        public GroceryList GetById(string id)
        {
            return db
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefault(x => (x.Id == id) && !x.Deleted);
        }

        public GroceryList GetByName(string name)
        {
            return db
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefault(x => (x.Name == name) && !x.Deleted);
        }

        public override int Create(GroceryList list)
        {
            list.CountId = 0;
            list.User = null;

            list.Id = Guid.NewGuid().ToString();

            db.Add(list);

            return context.SaveChanges();
        }

        public override int Update(GroceryList list)
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
                else db.Update(list);
            }

            return context.SaveChanges();
        }

        public override int Delete(GroceryList list)
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

        public override int ValidateOriginality(GroceryList obj)
        {
            return db.Any(x => (x.Name == obj.Name && x.UserId == obj.UserId) && !x.Deleted) ? -1 :
                db.Any(x => (x.Name == obj.Name && x.UserId == obj.UserId) && x.Deleted) ? -2 :
                0;
        }

        public override bool TryRestore(GroceryList obj)
        {
            GroceryList restorable = db.FirstOrDefault(x => (x.Name == obj.Name && x.UserId == obj.UserId) && x.Deleted);

            if (restorable == null) { return false; }

            db.Update(restorable);

            restorable.Deleted = false;

            context.SaveChanges();

            return true;
        }
    }
}
