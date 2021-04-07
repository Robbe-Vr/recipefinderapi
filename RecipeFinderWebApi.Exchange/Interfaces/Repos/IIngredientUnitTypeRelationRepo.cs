using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IIngredientUnitTypeRelationRepo
    {
        public IEnumerable<IngredientUnitTypeRelation> GetAll();

        public IngredientUnitTypeRelation GetById(int id);

        public IngredientUnitTypeRelation GetByIngredientIdAndUnitTypeId(string ingredientId, int unitTypeId);

        public int CreateRelation(Ingredient ingredient, UnitType unitType);

        public int DeleteRelation(IngredientUnitTypeRelation relation);
    }
}
