using Microsoft.EntityFrameworkCore;
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
                .Where(x => !x.Deleted);
        }

        public override IngredientCategory GetById(int id)
        {
            return db
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id && !x.Deleted);
        }

        public IngredientCategory GetByName(string name)
        {
            return db
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public override int Create(IngredientCategory category)
        {
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
    }
}
