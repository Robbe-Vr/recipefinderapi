using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class IngredientUnitTypeRelationRepo : AbstractBaseRelationRepo<IngredientUnitTypeRelation, Ingredient, UnitType>, IIngredientUnitTypeRelationRepo
    {
        public IngredientUnitTypeRelationRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.UnitTypesIngredient))
        {
        }

        public override IEnumerable<IngredientUnitTypeRelation> GetAll()
        {
            return db
                .Include(x => x.Ingredient)
                .Include(x => x.UnitType)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public override IngredientUnitTypeRelation GetById(int id)
        {
            return db
                .Include(x => x.Ingredient)
                .Include(x => x.UnitType)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id);
        }

        public IngredientUnitTypeRelation GetByIngredientIdAndUnitTypeId(string ingredientId, int unitTypeId)
        {
            return db
                .Include(x => x.Ingredient)
                .Include(x => x.UnitType)
                .AsNoTracking()
                .FirstOrDefault(x => x.IngredientId == ingredientId && x.UnitTypeId == unitTypeId);
        }

        public override int CreateRelation(IngredientUnitTypeRelation relation)
        {
            db.Add(relation);

            return context.SaveChanges();
        }

        public override int CreateRelation(Ingredient ingredient, UnitType unitType)
        {
            var relation = new IngredientUnitTypeRelation()
            {
                IngredientId = ingredient.Id,
                //Ingredient = ingredient,
                UnitTypeId = unitType.CountId,
                //UnitType = unitType,
                Deleted = false,
            };

            db.Add(relation);

            return context.SaveChanges();
        }

        public override int DeleteRelation(IngredientUnitTypeRelation relation)
        {
            if (!Exists(relation))
            {
                return 0;
            }
            if (!EntityIsAttached(relation))
            {
                if (KeyIsAttached(relation))
                {
                    relation = GetAttachedEntityByEntity(relation);
                }
            }

            relation.Ingredient = null;
            relation.UnitType = null;

            context.Entry(relation).State = EntityState.Deleted;

            return context.SaveChanges();
        }

        public override int DeleteRelation(Ingredient ingredient, UnitType unitType)
        {
            var relation = new IngredientUnitTypeRelation()
            {
                IngredientId = ingredient.Id,
                //Ingredient = ingredient,
                UnitTypeId = unitType.CountId,
                //UnitType = unitType,
                Deleted = false,
            };


            if (!Exists(relation))
            {
                return 0;
            }
            if (!EntityIsAttached(relation))
            {
                if (KeyIsAttached(relation))
                {
                    relation = GetAttachedEntityByEntity(relation);
                }
            }

            relation.Ingredient = null;
            relation.UnitType = null;

            context.Entry(relation).State = EntityState.Deleted;

            return context.SaveChanges();
        }

        public override int ValidateOriginality(IngredientUnitTypeRelation obj)
        {
            return db.Any(x => x.IngredientId == obj.IngredientId && x.UnitTypeId == obj.UnitTypeId && !x.Deleted) ? -1 :
                db.Any(x => x.IngredientId == obj.IngredientId && x.UnitTypeId == obj.UnitTypeId && x.Deleted) ? -2 :
                0;
        }

        public override bool TryRestore(IngredientUnitTypeRelation obj)
        {
            IngredientUnitTypeRelation restorable = db.FirstOrDefault(x => x.IngredientId == obj.IngredientId && x.UnitTypeId == obj.UnitTypeId && x.Deleted);

            if (restorable == null) { return false; }

            db.Update(restorable);

            restorable.Deleted = false;

            context.SaveChanges();

            return true;
        }
    }
}
