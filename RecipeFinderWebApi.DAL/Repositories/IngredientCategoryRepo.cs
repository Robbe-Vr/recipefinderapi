using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class IngredientCategoryRepo : AbstractRepo<IngredientCategory>, IIngredientCategoryRepo
    {
        public IngredientCategoryRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<IngredientCategory> GetAll()
        {
            return context.IngredientCategories
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public IngredientCategory GetById(int id)
        {
            return context.IngredientCategories
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id && !x.Deleted);
        }

        public IngredientCategory GetByName(string name)
        {
            return context.IngredientCategories
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public int Create(IngredientCategory category)
        {
            category.Ingredients = null;

            context.IngredientCategories.Add(category);

            return context.SaveChanges();
        }

        public int Update(IngredientCategory category)
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
                else context.IngredientCategories.Update(category);
            }

            return context.SaveChanges();
        }

        public int Delete(IngredientCategory category)
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
    }
}
