using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class KitchenRepo : AbstractRepo<KitchenIngredient>, IKitchenRepo
    {
        public KitchenRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<KitchenIngredient> GetAll()
        {
            return context.Kitchens
                .Include(k => k.Ingredient).ThenInclude(i => i.Categories)
                .Include(k => k.Ingredient).ThenInclude(i => i.UnitTypes)
                .Include(k => k.UnitType)
                .Include(k => k.User)
                .AsNoTracking()
                .Where(k => !k.Deleted);
        }

        public KitchenIngredient GetById(int id)
        {
            return context.Kitchens
                .Include(k => k.Ingredient).ThenInclude(i => i.Categories)
                .Include(k => k.Ingredient).ThenInclude(i => i.UnitTypes)
                .Include(k => k.UnitType)
                .Include(k => k.User)
                .AsNoTracking()
                .FirstOrDefault(k => k.CountId == id && !k.Deleted);
        }

        public Kitchen GetByUserId(string id)
        {
            var ingredients = context.Kitchens
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
            var ingredients = context.Kitchens
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

        public int Create(KitchenIngredient ingredient)
        {
            ingredient.Ingredient = null;
            ingredient.UnitType = null;
            ingredient.User = null;
            ingredient.Kitchens = null;

            context.Kitchens.Add(ingredient);

            return context.SaveChanges();
        }

        public int Update(KitchenIngredient ingredient)
        {
            context.Kitchens.Update(ingredient);

            return context.SaveChanges();
        }

        public int Delete(KitchenIngredient ingredient)
        {
            ingredient.Deleted = true;

            context.Kitchens.Update(ingredient);

            return context.SaveChanges();
        }
    }
}
