using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class IngredientCategoryRelationRepo : AbstractBaseRelationRepo<IngredientCategoryRelation, Ingredient, IngredientCategory>, IIngredientCategoryRelationRepo
    {
        public IngredientCategoryRelationRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.CategoriesIngredient))
        {
        }

        public override IEnumerable<IngredientCategoryRelation> GetAll()
        {
            return db
                .Include(x => x.Ingredient)
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public override IngredientCategoryRelation GetById(int id)
        {
            return db
                .Include(x => x.Ingredient)
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id);
        }

        public IngredientCategoryRelation GetByIngredientIdAndCategoryId(string ingredientId, int categoryId)
        {
            return db
                .Include(x => x.Ingredient)
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefault(x => x.IngredientId == ingredientId && x.CategoryId == categoryId);
        }

        public override int CreateRelation(IngredientCategoryRelation relation)
        {
            db.Add(relation);

            return context.SaveChanges();
        }

        public override int CreateRelation(Ingredient ingredient, IngredientCategory category)
        {
            var relation = new IngredientCategoryRelation()
            {
                IngredientId = ingredient.Id,
                //Ingredient = ingredient,
                CategoryId = category.CountId,
                //Category = category,
                Deleted = false,
            };

            db.Add(relation);

            return context.SaveChanges();
        }

        public override int DeleteRelation(Ingredient ingredient, IngredientCategory category)
        {
            var relation = new IngredientCategoryRelation()
            {
                IngredientId = ingredient.Id,
                //Ingredient = ingredient,
                CategoryId = category.CountId,
                //Category = category,
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
            relation.Category = null;

            context.Entry(relation).State = EntityState.Deleted;

            return context.SaveChanges();
        }

        public override int DeleteRelation(IngredientCategoryRelation relation)
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
            relation.Category = null;

            context.Entry(relation).State = EntityState.Deleted;

            return context.SaveChanges();
        }

        public override int ValidateOriginality(IngredientCategoryRelation obj)
        {
            return db.Any(x => x.IngredientId == obj.IngredientId && x.CategoryId == obj.CategoryId && !x.Deleted) ? -1 :
                db.Any(x => x.IngredientId == obj.IngredientId && x.CategoryId == obj.CategoryId && x.Deleted) ? -2 :
                0;
        }

        public override bool TryRestore(IngredientCategoryRelation obj)
        {
            IngredientCategoryRelation restorable = db.FirstOrDefault(x => x.IngredientId == obj.IngredientId && x.CategoryId == obj.CategoryId && x.Deleted);

            if (restorable == null) { return false; }

            db.Update(restorable);

            restorable.Deleted = false;

            context.SaveChanges();

            return true;
        }
    }
}
