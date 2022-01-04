using RecipeFinderWebApi.DAL.Mergers.Api.ExternalEndpoints;
using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.DAL.Mergers.Api.Ingredients
{
    internal class ExternalIngredients
    {
        public List<Ingredient> GetExternalIngredients()
        {
            List<Ingredient> list = new List<Ingredient>();

            list.AddRange(
                OpenFoodFacts.GetIngredients()
            );

            return list;
        }
    }
}
