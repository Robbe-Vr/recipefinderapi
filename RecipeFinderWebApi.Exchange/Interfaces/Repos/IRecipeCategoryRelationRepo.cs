using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IRecipeCategoryRelationRepo
    {
        public IEnumerable<RecipeCategoryRelation> GetAll();

        public RecipeCategoryRelation GetById(int id);

        public RecipeCategoryRelation GetByRecipeIdAndCategoryId(string recipeId, int categoryId);

        public int CreateRelation(Recipe recipe, RecipeCategory category);

        public int DeleteRelation(RecipeCategoryRelation relation);
    }
}
