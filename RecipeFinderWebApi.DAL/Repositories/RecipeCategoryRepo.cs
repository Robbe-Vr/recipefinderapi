using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using RecipeFinderWebApi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecipeFinderWebApi.DAL.Mergers;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class RecipeCategoryRepo : AbstractBaseEntityRepo<RecipeCategory>, IRecipeCategoryRepo
    {
        public RecipeCategoryRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.RecipeCategories))
        {
        }

        public override IEnumerable<RecipeCategory> GetAll()
        {
            return db
                .Include(x => x.Recipes)
                .AsNoTracking()
                .ToList().AddExternalRecipeCategories().Select((x, index) => { x.CountId = index; return x; }).Distinct()
                .Where(x => !x.Deleted);
        }

        public override RecipeCategory GetById(int id)
        {
            return db
                .Include(x => x.Recipes)
                .AsNoTracking()
                .ToList().AddExternalRecipeCategories().Select((x, index) => { x.CountId = index; return x; }).Distinct()
                .FirstOrDefault(x => (x.CountId == id) && !x.Deleted);
        }

        public RecipeCategory GetByName(string name)
        {
            return db
                .Include(x => x.Recipes)
                .AsNoTracking()
                .ToList().AddExternalRecipeCategories().Select((x, index) => { x.CountId = index; return x; }).Distinct()
                .FirstOrDefault(x => (x.Name == name) & !x.Deleted);
        }

        public override int Create(RecipeCategory category)
        {
            category.CountId = 0;
            category.Recipes = null;

            db.Add(category);

            return context.SaveChanges();
        }

        public override int Update(RecipeCategory category)
        {
            category.Recipes = null;

            if (!Exists(category))
            {
                return 0;
            }
            if (!EntityIsAttached(category))
            {
                if (KeyIsAttached(category))
                {
                    RecipeCategory old = GetAttachedEntityByEntity(category);
                    old.Name = category.Name;
                }
                else db.Update(category);
            }

            return context.SaveChanges();
        }

        public override int Delete(RecipeCategory category)
        {
            category.Recipes = null;

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

        public override int ValidateOriginality(RecipeCategory obj)
        {
            return db.Any(x => (x.Name == obj.Name) && !x.Deleted) ? -1 :
                db.Any(x => (x.Name == obj.Name) && x.Deleted) ? -2 :
                0;
        }

        public override bool TryRestore(RecipeCategory obj)
        {
            RecipeCategory restorable = db.FirstOrDefault(x => (x.Name == obj.Name) && x.Deleted);

            if (restorable == null) { return false; }

            db.Update(restorable);

            restorable.Deleted = false;

            context.SaveChanges();

            return true;
        }
    }
}
