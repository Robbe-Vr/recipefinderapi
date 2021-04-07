using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IIngredientCategoryRelationRepo
    {
        public IEnumerable<IngredientCategoryRelation> GetAll();

        public IngredientCategoryRelation GetById(int id);

        public IngredientCategoryRelation GetByIngredientIdAndCategoryId(string ingredientId, int categoryId);

        public int CreateRelation(Ingredient ingredient, IngredientCategory category);

        public int DeleteRelation(IngredientCategoryRelation relation);
    }
}
