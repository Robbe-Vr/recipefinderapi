using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL
{
    public class Seeding
    {
        private RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions);

        public void Seed()
        {
            try
            {
                if (context.Ingredients.Count() < 1)
                {
                    SeedIngredients();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed seeding the database! Reason: " + e.Message);
            }
        }

        private bool SeedIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>()
            {
                new Ingredient()
                {
                    Name = "Apple",
                },
                new Ingredient()
                {
                    Name = "Banana",
                },
                new Ingredient()
                {
                    Name = "Pear",
                },
            };

            context.Ingredients.AddRange(ingredients);
            int count = context.SaveChanges();

            return count == ingredients.Count;
        }
    }
}
