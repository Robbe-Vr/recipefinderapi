using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.DAL.Mergers;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class IngredientCategoryRepo : AbstractBaseEntityRepo<IngredientCategory>, IIngredientCategoryRepo
    {
        public IngredientCategoryRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.IngredientCategories))
        {
        }

        public override IEnumerable<IngredientCategory> GetAll()
        {
            return db
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .ToList().AddExternalIngredientCategories().Select((x, index) => { x.CountId = index; return x; }).Distinct()
                .Where(x => !x.Deleted);
        }

        public override IngredientCategory GetById(int id)
        {
            return db
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .ToList().AddExternalIngredientCategories().Select((x, index) => { x.CountId = index; return x; }).Distinct()
                .FirstOrDefault(x => (x.CountId == id) && !x.Deleted);
        }

        public IngredientCategory GetByName(string name)
        {
            return db
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .ToList().AddExternalIngredientCategories().Select((x, index) => { x.CountId = index; return x; }).Distinct()
                .FirstOrDefault(x => (x.Name == name) && !x.Deleted);
        }

        public override int Create(IngredientCategory category)
        {
            category.CountId = 0;
            category.Ingredients = null;

            db.Add(category);

            return context.SaveChanges();
        }

        public override int Update(IngredientCategory category)
        {
            category.Ingredients = null;

            if (!Exists(category))
            {
                return 0;
            }
            if (!EntityIsAttached(category))
            {
                if (KeyIsAttached(category))
                {
                    IngredientCategory old = GetAttachedEntityByEntity(category);
                    old.Name = category.Name;
                }
                else db.Update(category);
            }

            return context.SaveChanges();
        }

        public override int Delete(IngredientCategory category)
        {
            category.Ingredients = null;

            if (!Exists(category))
            {
                return 0;
            }
            if (!EntityIsAttached(category))
            {
                if (KeyIsAttached(category))
                {
                    category = GetAttachedEntityByEntity(category);
                }
            }

            context.Entry(category).State = EntityState.Modified;

            category.Deleted = true;

            return context.SaveChanges();
        }

        public override int ValidateOriginality(IngredientCategory obj)
        {
            return db.Any(x => (x.Name == obj.Name) && !x.Deleted) ? -1 :
                db.Any(x => (x.Name == obj.Name) && x.Deleted) ? -2 :
                0;
        }

        public override bool TryRestore(IngredientCategory obj)
        {
            IngredientCategory restorable = db.FirstOrDefault(x => (x.Name == obj.Name) && x.Deleted);

            if (restorable == null) { return false; }

            db.Update(restorable);

            restorable.Deleted = false;

            context.SaveChanges();

            return true;
        }
    }
}
