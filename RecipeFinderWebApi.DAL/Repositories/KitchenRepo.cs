using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class KitchenRepo : AbstractBaseEntityRepo<KitchenIngredient>, IKitchenRepo
    {
        public KitchenRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.Kitchens))
        {
        }

        public override IEnumerable<KitchenIngredient> GetAll()
        {
            return db
                .Include(k => k.Ingredient).ThenInclude(i => i.Categories)
                .Include(k => k.Ingredient).ThenInclude(i => i.UnitTypes)
                .Include(k => k.UnitType)
                .Include(k => k.User)
                .AsNoTracking()
                .Where(k => !k.Deleted);
        }

        public override KitchenIngredient GetById(int id)
        {
            return db
                .Include(k => k.Ingredient).ThenInclude(i => i.Categories)
                .Include(k => k.Ingredient).ThenInclude(i => i.UnitTypes)
                .Include(k => k.UnitType)
                .Include(k => k.User)
                .AsNoTracking()
                .FirstOrDefault(k => k.CountId == id && !k.Deleted);
        }

        public Kitchen GetByUserId(string id)
        {
            var ingredients = db
                .Include(k => k.Ingredient).ThenInclude(i => i.Categories)
                .Include(k => k.Ingredient).ThenInclude(i => i.UnitTypes)
                .Include(k => k.UnitType)
                .Include(k => k.User)
                .AsNoTracking()
                .Where(k => k.UserId == id && !k.Deleted);

            var user = ingredients.FirstOrDefault()?.User;

            return new Kitchen()
            {
                Ingredients = ingredients.ToArray(),
                User = user,
                UserId = id,
            };
        }

        public Kitchen GetByUserName(string name)
        {
            var ingredients = db
                 .Include(k => k.Ingredient).ThenInclude(i => i.Categories)
                 .Include(k => k.Ingredient).ThenInclude(i => i.UnitTypes)
                 .Include(k => k.UnitType)
                 .Include(k => k.User)
                 .AsNoTracking()
                 .Where(k => k.User.Name == name && !k.Deleted);

            var user = ingredients.FirstOrDefault().User;

            return new Kitchen()
            {
                Ingredients = ingredients.ToArray(),
                User = user,
                UserId = user.Id,
            };
        }

        public override int Create(KitchenIngredient ingredient)
        {
            ingredient.User = null;
            ingredient.Ingredient = null;
            ingredient.UnitType = null;

            db.Add(ingredient);

            return context.SaveChanges();
        }

        public override int Update(KitchenIngredient ingredient)
        {
            ingredient.User = null;
            ingredient.Ingredient = null;
            ingredient.UnitType = null;

            if (!Exists(ingredient))
            {
                return 0;
            }
            if (!EntityIsAttached(ingredient))
            {
                if (KeyIsAttached(ingredient))
                {
                    KitchenIngredient old = GetAttachedEntityByEntity(ingredient);
                    old.Units = ingredient.Units;
                    old.UnitTypeId = ingredient.UnitTypeId;
                }
                else db.Update(ingredient);
            }

            return context.SaveChanges();
        }

        public override int Delete(KitchenIngredient ingredient)
        {
            ingredient.User = null;
            ingredient.Ingredient = null;
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
    }
}
