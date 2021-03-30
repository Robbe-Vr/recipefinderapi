using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class IngredientRepo : IIngredientRepo
    {
        private RecipeFinderDBContext context = new RecipeFinderDBContext(RecipeFinderDBContext.ops.dbOptions);

        public IEnumerable<Ingredient> GetAll()
        {
            return context.Ingredients
                .Include(x => x.Categories)
                .Include(x => x.UnitTypes)
                .Where(x => !x.Deleted);
        }

        public Ingredient GetById(string id)
        {
            return context.Ingredients
                .Include(x => x.Categories)
                .Include(x => x.UnitTypes)
                .FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public Ingredient GetByName(string name)
        {
            return context.Ingredients
                .Include(x => x.Categories)
                .Include(x => x.UnitTypes)
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public int Create(Ingredient ingredient)
        {
            context.Ingredients.Add(ingredient);

            return context.SaveChanges();
        }

        public int Update(Ingredient ingredient)
        {
            context.Ingredients.Update(ingredient);

            return context.SaveChanges();
        }

        public int Delete(Ingredient ingredient)
        {
            ingredient.Deleted = true;

            context.Ingredients.Update(ingredient);

            return context.SaveChanges();
        }
    }
}
