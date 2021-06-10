using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using RecipeFinderWebApi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                .Where(x => !x.Deleted);
        }

        public override RecipeCategory GetById(int id)
        {
            return db
                .Include(x => x.Recipes)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id && !x.Deleted);
        }

        public RecipeCategory GetByName(string name)
        {
            return db
                .Include(x => x.Recipes)
                .AsNoTracking()
                .FirstOrDefault(x => x.Name == name & !x.Deleted);
        }

        public override int Create(RecipeCategory category)
        {
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
    }
}
