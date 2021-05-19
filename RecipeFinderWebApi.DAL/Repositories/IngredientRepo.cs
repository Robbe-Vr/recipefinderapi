using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class IngredientRepo : AbstractRepo<Ingredient>, IIngredientRepo
    {
        public IngredientRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Ingredient> GetAll()
        {
            return context.Ingredients
                .Include(x => x.Categories)
                .Include(x => x.UnitTypes)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public Ingredient GetById(string id)
        {
            return context.Ingredients
                .Include(x => x.Categories)
                .Include(x => x.UnitTypes)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public Ingredient GetByName(string name)
        {
            return context.Ingredients
                .Include(x => x.Categories)
                .Include(x => x.UnitTypes)
                .AsNoTracking()
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public int Create(Ingredient ingredient)
        {
            ingredient.UnitTypes = null;
            ingredient.Categories = null;

            ingredient.Id = Guid.NewGuid().ToString();

            context.Ingredients.Add(ingredient);

            return context.SaveChanges();
        }

        public Ingredient CreateGetId(Ingredient ingredient)
        {
            ingredient.UnitTypes = null;
            ingredient.Categories = null;

            ingredient.Id = Guid.NewGuid().ToString();

            context.Ingredients.Add(ingredient);

            context.SaveChanges();

            return ingredient;
        }

        public int Update(Ingredient ingredient)
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
                }
                else context.Ingredients.Update(ingredient);
            }

            return context.SaveChanges();
        }

        public int Delete(Ingredient ingredient)
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
                else context.Ingredients.Update(ingredient);
            }

            context.Entry(ingredient).State = EntityState.Modified;

            ingredient.Deleted = true;

            return context.SaveChanges();
        }
    }
}
