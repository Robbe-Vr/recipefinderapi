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

        public int Create(RecipeCategory recipe)
        {
            recipe.Recipes = null;

            context.RecipeCategories.Add(recipe);

            return context.SaveChanges();
        }

        public int Update(RecipeCategory recipe)
        {
            context.RecipeCategories.Update(recipe);

            return context.SaveChanges();
        }

        public int Delete(RecipeCategory recipe)
        {
            recipe.Deleted = true;

            context.RecipeCategories.Update(recipe);

            return context.SaveChanges();
        }
    }
}
