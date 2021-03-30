using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class IngredientCategoryRepo : IIngredientCategoryRepo
    {
        private RecipeFinderDBContext context = new RecipeFinderDBContext(RecipeFinderDBContext.ops.dbOptions);

        public IEnumerable<IngredientCategory> GetAll()
        {
            return context.IngredientCategories
                .Include(x => x.Ingredients)
                .Where(x => !x.Deleted);
        }

        public IngredientCategory GetById(int id)
        {
            return context.IngredientCategories
                .Include(x => x.Ingredients)
                .FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public IngredientCategory GetByName(string name)
        {
            return context.IngredientCategories
                .Include(x => x.Ingredients)
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public int Create(IngredientCategory ingredient)
        {
            context.IngredientCategories.Add(ingredient);

            return context.SaveChanges();
        }

        public int Update(IngredientCategory ingredient)
        {
            context.IngredientCategories.Update(ingredient);

            return context.SaveChanges();
        }

        public int Delete(IngredientCategory ingredient)
        {
            ingredient.Deleted = true;

            context.IngredientCategories.Update(ingredient);

            return context.SaveChanges();
        }
    }
}
