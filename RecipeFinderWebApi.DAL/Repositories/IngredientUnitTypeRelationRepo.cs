using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class IngredientUnitTypeRelationRepo : AbstractRepo<IngredientUnitTypeRelation>, IIngredientUnitTypeRelationRepo
    {
        public IngredientUnitTypeRelationRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<IngredientUnitTypeRelation> GetAll()
        {
            return context.UnitTypesIngredient
                .Include(x => x.Ingredient)
                .Include(x => x.UnitType)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public IngredientUnitTypeRelation GetById(int id)
        {
            return context.UnitTypesIngredient
                .Include(x => x.Ingredient)
                .Include(x => x.UnitType)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id);
        }

        public IngredientUnitTypeRelation GetByIngredientIdAndUnitTypeId(string ingredientId, int unitTypeId)
        {
            return context.UnitTypesIngredient
                .Include(x => x.Ingredient)
                .Include(x => x.UnitType)
                .AsNoTracking()
                .FirstOrDefault(x => x.IngredientId == ingredientId && x.UnitTypeId == unitTypeId);
        }

        public int CreateRelation(Ingredient ingredient, UnitType unitType)
        {
            var relation = new IngredientUnitTypeRelation()
            {
                IngredientId = ingredient.Id,
                //Ingredient = ingredient,
                UnitTypeId = unitType.CountId,
                //UnitType = unitType,
                Deleted = false,
            };

            context.UnitTypesIngredient
                .Add(relation);

            return context.SaveChanges();
        }

        public int DeleteRelation(IngredientUnitTypeRelation relation)
        {
            context.UnitTypesIngredient
                .Remove(relation);

            return context.SaveChanges();
        }
    }
}
