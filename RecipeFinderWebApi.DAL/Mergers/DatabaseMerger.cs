using RecipeFinderWebApi.DAL.Mergers.Api.IngredientCategories;
using RecipeFinderWebApi.DAL.Mergers.Api.Ingredients;
using RecipeFinderWebApi.DAL.Mergers.Api.RecipeCategories;
using RecipeFinderWebApi.DAL.Mergers.Api.Recipes;
using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.DAL.Mergers
{
    internal static class DatabaseMerger
    {
        private static readonly ExternalIngredientCategories externalIngredientCategories = new();
        private static readonly ExternalRecipeCategories externalRecipeCategories = new();
        private static readonly ExternalIngredients externalIngredients = new();
        private static readonly ExternalRecipes externalRecipes = new();

        public static List<Ingredient> AddExternalIngredients(this List<Ingredient> list)
        {
            if (!ExternalDatabaseSettings.LoadExternalDatabases) { return list; }

            int minId = list.Max(x => x.CountId) + 1;

            list.AddRange(
                    externalIngredients.GetExternalIngredients().Select((x, index) => { x.CountId = minId + index; return x; })
            );

            return list.Distinct().ToList();
        }

        public static List<IngredientCategory> AddExternalIngredientCategories(this List<IngredientCategory> list)
        {
            if (!ExternalDatabaseSettings.LoadExternalDatabases) { return list; }

            int minId = list.Max(x => x.CountId) + 1;

            list.AddRange(
                    externalIngredientCategories.GetExternalIngredientCategories().Select((x, index) => { x.CountId = minId + index; return x; })
            );

            return list.Distinct().ToList();
        }

        public static List<Recipe> AddExternalRecipes(this List<Recipe> list)
        {
            if (!ExternalDatabaseSettings.LoadExternalDatabases) { return list; }

            int minId = list.Max(x => x.CountId) + 1;

            list.AddRange(
                    externalRecipes.GetExternalRecipes().Select((x, index) => { x.CountId = minId + index; return x; })
            );

            return list.Distinct().ToList();
        }

        public static List<RecipeCategory> AddExternalRecipeCategories(this List<RecipeCategory> list)
        {
            if (!ExternalDatabaseSettings.LoadExternalDatabases) { return list; }

            int minId = list.Max(x => x.CountId) + 1;

            list.AddRange(
                    externalRecipeCategories.GetExternalRecipeCategories().Select((x, index) => { x.CountId = minId + index; return x; })
            );

            return list.Distinct().ToList();
        }
    }
}
