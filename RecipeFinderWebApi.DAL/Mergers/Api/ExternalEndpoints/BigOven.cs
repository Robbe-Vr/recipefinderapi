using Newtonsoft.Json;
using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecipeFinderWebApi.DAL.Mergers.Api.ExternalEndpoints.BigOven.BigOvenResponse;

namespace RecipeFinderWebApi.DAL.Mergers.Api.ExternalEndpoints
{
    internal static class BigOven
    {
        private static readonly string api_key = "";

        public static IEnumerable<Recipe> GetRecipes()
        {
            Task<string> task = ExternalApiClient.SendRequest(
                "https://api2.bigoven.com/recipes",
                new ExternalApiClient.Param[]
                {
                    new ExternalApiClient.Param("api_key", api_key),
                }
            );
            task.Wait();
            string jsonStr = task.Result;

            try
            {
                if (!String.IsNullOrEmpty(jsonStr) || !jsonStr.Contains("Error"))
                {
                    BigOvenResponse data = JsonConvert.DeserializeObject<BigOvenResponse>(jsonStr);

                    List<Recipe> recipes = new List<Recipe>();

                    foreach (BigOvenRecipe apiRecipe in data.Results)
                    {
                        recipes.Add(new Recipe()
                        {
                            Id = apiRecipe.RecipeID.ToString(),
                            Name = apiRecipe.Title,
                            ImageLocation = apiRecipe.ImageUrl,
                            Description = apiRecipe.Description,
                            PreparationSteps = String.Join("{NEXT}", apiRecipe.Instructions.Split(".", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())),
                            IsPublic = true,
                            User = new User() { Id = apiRecipe.Poster.UserID.ToString(), Name = apiRecipe.Poster.UserName,  },
                            Deleted = false,
                        });
                    }

                    return recipes;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed to parse json data from external api 'BigOven'! Reason: " + e.Message);
            }

            return new List<Recipe>();
        }

        internal class BigOvenResponse
        {
            public List<BigOvenRecipe> Results { get; set; }

            internal class BigOvenRecipe
            {
                public int RecipeID { get; set; }
                public string Title { get; set; }
                public string Description { get; set; }
                public string Cuisine { get; set; }
                public string Category { get; set; }
                public string Subcategory { get; set; }
                public string Microcategory { get; set; }
                public string PrimaryIngredient { get; set; }
                public double StarRating { get; set; }
                public string WebUrl { get; set; }
                public string ImageUrl { get; set; }
                public int ReviewCount { get; set; }
                public int MedalCount { get; set; }
                public int FavoriteCount { get; set; }
                public BigOvenPoster Poster { get; set; }
                public List<BigOvenIngredient> Ingredients { get; set; }
                public string Instructions { get; set; }

                internal class BigOvenPoster
                {
                    public int UserID { get; set; }
                    public string UserName { get; set; }
                    public string PhotoUrl { get; set; }
                    public string IsPremium { get; set; }
                    public string IsKitchenHelper { get; set; }
                }

                internal class BigOvenIngredient
                {
                    public int IngredientID { get; set; }
                    public int DisplayIndex { get; set; }
                    public bool IsHeading { get; set; }
                    public string Name { get; set; }
                    public double Quantity { get; set; }
                    public string DisplayQuantity { get; set; }
                    public string Unit { get; set; }
                    public int MetricQuantity { get; set; }
                    public string MetricDisplayQuantity { get; set; }
                    public string MetricUnit { get; set; }
                    public string PreparationNotes { get; set; }
                    public BigOvenIngredientInfo IngredientInfo { get; set; }
                    public bool IsLinked { get; set; }

                    internal class BigOvenIngredientInfo
                    {
                        public string Name { get; set; }
                        public string Department { get; set; }
                    }
                }
            }
        }
    }
}
