using Newtonsoft.Json;
using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecipeFinderWebApi.DAL.Mergers.Api.ExternalEndpoints.OpenFoodFacts.OpenFoodFactsResponse;

namespace RecipeFinderWebApi.DAL.Mergers.Api.ExternalEndpoints
{
    internal static class OpenFoodFacts
    {
        public static IEnumerable<IngredientCategory> GetIngredientCategories()
        {
            Task<string> task = ExternalApiClient.SendRequest("https://world.openfoodfacts.org/categories.json");
            task.Wait();
            string jsonStr = task.Result;

            try
            {
                if (!String.IsNullOrEmpty(jsonStr))
                {
                    OpenFoodFactsResponse data = JsonConvert.DeserializeObject<OpenFoodFactsResponse>(jsonStr);

                    List<IngredientCategory> categories = new List<IngredientCategory>();

                    foreach (OpenFoodFactsObject apiCategory in data.tags)
                    {
                        categories.Add(new IngredientCategory()
                        {
                            Name = apiCategory.name,
                            CountId = 0,
                            Deleted = false,
                        });
                    }

                    return categories;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed to parse json data from external api 'BigOven'! Reason: " + e.Message);
            }

            return new List<IngredientCategory>();
        }


        public static IEnumerable<Ingredient> GetIngredients()
        {
            Task<string> task = ExternalApiClient.SendRequest("https://world.openfoodfacts.org/ingredients.json");
            task.Wait();
            string jsonStr = task.Result;

            try
            {
                if (!String.IsNullOrEmpty(jsonStr))
                {
                    OpenFoodFactsResponse data = JsonConvert.DeserializeObject<OpenFoodFactsResponse>(jsonStr);

                    List<Ingredient> ingredients = new List<Ingredient>();

                    foreach (OpenFoodFactsObject apiIngredient in data.tags)
                    {
                        ingredients.Add(new Ingredient()
                        {
                            Name = apiIngredient.name,
                            Id = apiIngredient.id,
                        });
                    }

                    return ingredients;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed to parse json data from external api 'BigOven'! Reason: " + e.Message);
            }

            return new List<Ingredient>();
        }

        internal class OpenFoodFactsResponse
        {
            public int count { get; set; }

            public List<OpenFoodFactsObject> tags { get; set; }

            internal class OpenFoodFactsObject
            {
                public string id { get; set; }
                public int known { get; set; }
                public string name { get; set; }
                public int products { get; set; }
                public List<string> sameAs { get; set; }
                public string url { get; set; }
            }
        }
    }
}
