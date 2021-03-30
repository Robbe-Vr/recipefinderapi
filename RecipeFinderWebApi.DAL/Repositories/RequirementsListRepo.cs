using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class RequirementsListRepo : IRequirementsListRepo
    {
        private RecipeFinderDBContext context = new RecipeFinderDBContext(RecipeFinderDBContext.ops.dbOptions);

        public IEnumerable<RequirementsListIngredient> GetAll()
        {
            return context.RequirementsLists
                .Include(x => x.Ingredient).ThenInclude(x => x.Categories)
                .Include(x => x.Ingredient).ThenInclude(x => x.UnitTypes)
                .Include(x => x.UnitType)
                .Include(x => x.Recipe)
                .Where(x => !x.Deleted);
        }

        public RequirementsList GetById(string id)
        {
            var ingredients = context.RequirementsLists
                .Include(x => x.Ingredient).ThenInclude(x => x.Categories)
                .Include(x => x.Ingredient).ThenInclude(x => x.UnitTypes)
                .Include(x => x.UnitType)
                .Include(x => x.Recipe)
                .Where(x => x.RecipeId == id && !x.Deleted);

            var recipe = ingredients.FirstOrDefault()?.Recipe;

            return new RequirementsList()
            {
                Recipe = recipe,
                RecipeId = recipe.Id,
                Ingredients = ingredients.ToArray(),
            };
        }

        public RequirementsList GetByName(string name)
        {
            var ingredients = context.RequirementsLists
                .Include(x => x.Ingredient).ThenInclude(x => x.Categories)
                .Include(x => x.Ingredient).ThenInclude(x => x.UnitTypes)
                .Include(x => x.UnitType)
                .Include(x => x.Recipe)
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
