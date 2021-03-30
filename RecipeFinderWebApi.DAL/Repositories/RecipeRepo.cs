using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class RecipeRepo : IRecipeRepo
    {
        private RecipeFinderDBContext context = new RecipeFinderDBContext(RecipeFinderDBContext.ops.dbOptions);

        public IEnumerable<Recipe> GetAll()
        {
            return context.Recipes
                .Include(x => x.Categories)
                .Include(x => x.User)
                .Include(x => x.RequirementsList.Ingredients)
                    .ThenInclude(x => x.Ingredient)
                .Include(x => x.RequirementsList.Ingredients)
                    .ThenInclude(x => x.UnitType)
                .Where(x => !x.Deleted);
        }

        public Recipe GetById(string id)
        {
            return context.Recipes
                .Include(x => x.Categories)
                .Include(x => x.User)
                .Include(x => x.RequirementsList.Ingredients)
                    .ThenInclude(x => x.Ingredient)
                .Include(x => x.RequirementsList.Ingredients)
                    .ThenInclude(x => x.UnitType)
                .FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public Recipe GetByName(string name)
        {
            return context.Recipes
                .Include(x => x.Categories)
                .Include(x => x.User)
                .Include(x => x.RequirementsList.Ingredients)
                    .ThenInclude(x => x.Ingredient)
                .Include(x => x.RequirementsList.Ingredients)
                    .ThenInclude(x => x.UnitType)
                .FirstOrDefault(x => x.Name == name & !x.Deleted);
        }

        public int Create(Recipe recipe)
        {
            context.Recipes.Add(recipe);

            return context.SaveChanges();
        }

        public int Update(Recipe recipe)
        {
            context.Recipes.Update(recipe);

            return context.SaveChanges();
        }

        public int Delete(Recipe recipe)
        {
            recipe.Deleted = true;

            context.Recipes.Update(recipe);

            return context.SaveChanges();
        }
    }
}
