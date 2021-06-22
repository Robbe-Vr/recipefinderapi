using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IIngredientCategoryRelationRepo : IBaseRelationRepo<IngredientCategoryRelation, Ingredient, IngredientCategory>
    {

        public IngredientCategoryRelation GetByIngredientIdAndCategoryId(string ingredientId, int categoryId);
    }
}
