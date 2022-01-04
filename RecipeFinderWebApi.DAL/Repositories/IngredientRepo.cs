using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.DAL.Mergers;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class IngredientRepo : AbstractBaseEntityRepo<Ingredient>, IIngredientRepo
    {
        public IngredientRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.Ingredients))
        {
        }

        public override IEnumerable<Ingredient> GetAll()
        {
            return db
                .Include(x => x.Categories)
                .Include(x => x.UnitTypes)
                .AsNoTracking()
                .ToList().AddExternalIngredients()
                .Where(x => !x.Deleted);
        }

        public override Ingredient GetById(int id)
        {
            return db
                .Include(x => x.Categories)
                .Include(x => x.UnitTypes)
                .AsNoTracking()
                .ToList().AddExternalIngredients()
                .FirstOrDefault(x => (x.CountId == id) && !x.Deleted);
        }

        public Ingredient GetById(string id)
        {
            return db
                .Include(x => x.Categories)
                .Include(x => x.UnitTypes)
                .AsNoTracking()
                .ToList().AddExternalIngredients()
                .FirstOrDefault(x => (x.Id == id) && !x.Deleted);
        }

        public Ingredient GetByName(string name)
        {
            return db
                .Include(x => x.Categories)
                .Include(x => x.UnitTypes)
                .AsNoTracking()
                .ToList().AddExternalIngredients()
                .FirstOrDefault(x => (x.Name == name) && !x.Deleted);
        }

        public override int Create(Ingredient ingredient)
        {
            ingredient.CountId = 0;
            ingredient.UnitTypes = null;
            ingredient.Categories = null;

            ingredient.Id = Guid.NewGuid().ToString();

            db.Add(ingredient);

            return context.SaveChanges();
        }

        public Ingredient CreateGetId(Ingredient ingredient)
        {
            ingredient.UnitTypes = null;
            ingredient.Categories = null;

            ingredient.Id = Guid.NewGuid().ToString();

            db.Add(ingredient);

            context.SaveChanges();

            return ingredient;
        }

        public override int Update(Ingredient ingredient)
        {
            ingredient.UnitTypes = null;
            ingredient.Categories = null;

            if (!Exists(ingredient))
            {
                return 0;
            }
            if (!EntityIsAttached(ingredient))
            {
                if (KeyIsAttached(ingredient))
                {
                    Ingredient old = GetAttachedEntityByEntity(ingredient);

                    old.Name = ingredient.Name;
                    old.ImageLocation = ingredient.ImageLocation;
                    old.Deleted = ingredient.Deleted;
                    old.AverageVolumeInLiterPerUnit = ingredient.AverageVolumeInLiterPerUnit;
                    old.AverageWeightInKgPerUnit = ingredient.AverageWeightInKgPerUnit;

                    db.Update(old);
                }
                else db.Update(ingredient);
            }

            return context.SaveChanges();
        }

        public override int Delete(Ingredient ingredient)
        {
            ingredient.UnitTypes = null;
            ingredient.Categories = null;

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

            context.Entry(ingredient).State = EntityState.Modified;

            ingredient.Deleted = true;

            return context.SaveChanges();
        }

        public override int ValidateOriginality(Ingredient obj)
        {
            return db.Any(x => (x.Name == obj.Name) && !x.Deleted) ? -1 :
                db.Any(x => (x.Name == obj.Name) && x.Deleted) ? -2 :
                0;
        }

        public override bool TryRestore(Ingredient obj)
        {
            Ingredient restorable = db.FirstOrDefault(x => (x.Name == obj.Name) && x.Deleted);

            if (restorable == null) { return false; }

            db.Update(restorable);

            restorable.Deleted = false;

            context.SaveChanges();

            return true;
        }
    }
}
