using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IIngredientUnitTypeRelationRepo : IBaseRelationRepo<IngredientUnitTypeRelation, Ingredient, UnitType>
    {
        public IngredientUnitTypeRelation GetByIngredientIdAndUnitTypeId(string ingredientId, int unitTypeId);
    }
}
