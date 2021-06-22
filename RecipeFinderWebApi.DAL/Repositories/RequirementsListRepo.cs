using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class RequirementsListRepo : AbstractBaseEntityRepo<RequirementsListIngredient>, IRequirementsListRepo
    {
        public RequirementsListRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.RequirementsLists))
        {
        }

        public override IEnumerable<RequirementsListIngredient> GetAll()
        {
            return db
                .Include(x => x.Ingredient).ThenInclude(x => x.Categories)
                .Include(x => x.Ingredient).ThenInclude(x => x.UnitTypes)
                .Include(x => x.UnitType)
                .Include(x => x.Recipe)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public override RequirementsListIngredient GetById(int id)
        {
            return db
                .Include(x => x.Ingredient).ThenInclude(x => x.Categories)
                .Include(x => x.Ingredient).ThenInclude(x => x.UnitTypes)
                .Include(x => x.UnitType)
                .Include(x => x.Recipe)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id && !x.Deleted);
        }

        public IEnumerable<RequirementsListIngredient> GetByRecipeId(string id)
        {
            var ingredients = db
                .Include(x => x.Ingredient).ThenInclude(x => x.Categories)
                .Include(x => x.Ingredient).ThenInclude(x => x.UnitTypes)
                .Include(x => x.UnitType)
                .Include(x => x.Recipe)
                .AsNoTracking()
                .Where(x => x.RecipeId == id && !x.Deleted);

            return ingredients;
        }

        public IEnumerable<RequirementsListIngredient> GetByRecipeName(string name)
        {
            var ingredients = db
                .Include(x => x.Ingredient).ThenInclude(x => x.Categories)
                .Include(x => x.Ingredient).ThenInclude(x => x.UnitTypes)
                .Include(x => x.UnitType)
                .Include(x => x.Recipe)
                .AsNoTracking()
                .Where(x => x.Recipe.Name == name && !x.Deleted);

            return ingredients;
        }

        public override int Create(RequirementsListIngredient ingredient)
        {
            ingredient.CountId = 0;
            ingredient.Ingredient = null;
            ingredient.Recipe = null;
            ingredient.UnitType = null;

            db.Add(ingredient);

            return context.SaveChanges();
        }

        public override int Update(RequirementsListIngredient ingredient)
        {
            ingredient.Ingredient = null;
            ingredient.Recipe = null;
            ingredient.UnitType = null;

            if (!Exists(ingredient))
            {
                return 0;
            }
            if (!EntityIsAttached(ingredient))
            {
                if (KeyIsAttached(ingredient))
                {
                    RequirementsListIngredient old = GetAttachedEntityByEntity(ingredient);
                    old.Units = ingredient.Units;
                    old.UnitTypeId = ingredient.UnitTypeId;
                }
                else db.Update(ingredient);
            }

            return context.SaveChanges();
        }

        public override int Delete(RequirementsListIngredient ingredient)
        {
            ingredient.Ingredient = null;
            ingredient.Recipe = null;
            ingredient.UnitType = null;

            if (!Exists(ingredient))
            {
                return 0;
            }
            if (!EntityIsAttached(ingredient))
            {
                if (KeyIsAttached(ingredient))
                {
                    ingredient = GetAttachedEntityByEntity(ingredient);
                }
                else db.Update(ingredient);
            }

            ingredient.Deleted = true;

            return context.SaveChanges();
        }

        public override int ValidateOriginality(RequirementsListIngredient obj)
        {
            return db.Any(x => x.RecipeId == obj.RecipeId && x.IngredientId == obj.IngredientId && !x.Deleted) ? -1 :
                db.Any(x => x.RecipeId == obj.RecipeId && x.IngredientId == obj.IngredientId && x.Deleted) ? -2 :
                0;
        }

        public override bool TryRestore(RequirementsListIngredient obj)
        {
            RequirementsListIngredient restorable = db.FirstOrDefault(x => x.RecipeId == obj.RecipeId && x.IngredientId == obj.IngredientId && x.Deleted);

            if (restorable == null) { return false; }

            db.Update(restorable);

            restorable.Deleted = false;
            restorable.UnitTypeId = obj.UnitTypeId;
            restorable.UnitType = null;
            restorable.Units = obj.Units;

            context.SaveChanges();

            return true;
        }
    }
}
