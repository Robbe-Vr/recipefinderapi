using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IRecipeCategoryRelationRepo : IBaseRelationRepo<RecipeCategoryRelation, Recipe, RecipeCategory>
    {
        public RecipeCategoryRelation GetByRecipeIdAndCategoryId(string recipeId, int categoryId);
    }
}
