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
    public class RecipeCategoryRepo : AbstractRepo<RecipeCategory>, IRecipeCategoryRepo
    {
        public RecipeCategoryRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<RecipeCategory> GetAll()
        {
            return context.RecipeCategories
                .Include(x => x.Recipes)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public RecipeCategory GetById(int id)
        {
            return context.RecipeCategories
                .Include(x => x.Recipes)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id && !x.Deleted);
        }

        public RecipeCategory GetByName(string name)
        {
            return context.RecipeCategories
                .Include(x => x.Recipes)
                .AsNoTracking()
                .FirstOrDefault(x => x.Name == name & !x.Deleted);
        }

        public int Create(RecipeCategory category)
        {
            category.Recipes = null;

            context.RecipeCategories.Add(category);

            return context.SaveChanges();
        }

        public int Update(RecipeCategory category)
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
                else context.RecipeCategories.Update(category);
            }

            return context.SaveChanges();
        }

        public int Delete(RecipeCategory category)
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
