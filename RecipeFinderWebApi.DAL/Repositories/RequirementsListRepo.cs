using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class RequirementsListRepo : AbstractRepo<RequirementsListIngredient>, IRequirementsListRepo
    {
        public RequirementsListRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<RequirementsListIngredient> GetAll()
        {
            return context.RequirementsLists
                .Include(x => x.Ingredient).ThenInclude(x => x.Categories)
                .Include(x => x.Ingredient).ThenInclude(x => x.UnitTypes)
                .Include(x => x.UnitType)
                .Include(x => x.Recipe)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public RequirementsListIngredient GetById(int id)
        {
            return context.RequirementsLists
                .Include(x => x.Ingredient).ThenInclude(x => x.Categories)
                .Include(x => x.Ingredient).ThenInclude(x => x.UnitTypes)
                .Include(x => x.UnitType)
                .Include(x => x.Recipe)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id && !x.Deleted);
        }

        public RequirementsList GetByRecipeId(string id)
        {
            var ingredients = context.RequirementsLists
                .Include(x => x.Ingredient).ThenInclude(x => x.Categories)
                .Include(x => x.Ingredient).ThenInclude(x => x.UnitTypes)
                .Include(x => x.UnitType)
                .Include(x => x.Recipe)
                .AsNoTracking()
                .Where(x => x.RecipeId == id && !x.Deleted);

            var recipe = ingredients.FirstOrDefault()?.Recipe;

            return new RequirementsList()
            {
                Recipe = recipe,
                RecipeId = recipe.Id,
                Ingredients = ingredients.ToArray(),
            };
        }

        public RequirementsList GetByRecipeName(string name)
        {
            var ingredients = context.RequirementsLists
                .Include(x => x.Ingredient).ThenInclude(x => x.Categories)
                .Include(x => x.Ingredient).ThenInclude(x => x.UnitTypes)
                .Include(x => x.UnitType)
                .Include(x => x.Recipe)
                .AsNoTracking()
                .Where(x => x.Recipe.Name == name && !x.Deleted);

            var recipe = ingredients.FirstOrDefault()?.Recipe;

            return new RequirementsList()
            {
                Recipe = recipe,
                RecipeId = recipe.Id,
                Ingredients = ingredients.ToArray(),
            };
        }

        public int Create(RequirementsListIngredient ingredient)
        {
            ingredient.Ingredient = null;
            ingredient.Recipe = null;
            ingredient.UnitType = null;

            context.RequirementsLists.Add(ingredient);

            return context.SaveChanges();
        }

        public int Update(RequirementsListIngredient ingredient)
        {
            context.RequirementsLists.Update(ingredient);

            return context.SaveChanges();
        }

        public int Delete(RequirementsListIngredient ingredient)
        {
            ingredient.Deleted = true;

            context.RequirementsLists.Update(ingredient);

            return context.SaveChanges();
        }
    }
}
