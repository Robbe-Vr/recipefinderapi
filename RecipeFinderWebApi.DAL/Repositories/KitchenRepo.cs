using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class KitchenRepo : IKitchenRepo
    {
        private RecipeFinderDBContext context = new RecipeFinderDBContext(RecipeFinderDBContext.ops.dbOptions);

        public IEnumerable<KitchenIngredient> GetAll()
        {
            return context.Kitchens
                .Include(k => k.Ingredient).ThenInclude(i => i.Categories)
                .Include(k => k.Ingredient).ThenInclude(i => i.UnitTypes)
                .Include(k => k.UnitType)
                .Include(k => k.User)
                .Where(k => !k.Deleted);
        }

        public Kitchen GetById(string id)
        {
            var ingredients = context.Kitchens
                .Include(k => k.Ingredient).ThenInclude(i => i.Categories)
                .Include(k => k.Ingredient).ThenInclude(i => i.UnitTypes)
                .Include(k => k.UnitType)
                .Include(k => k.User)
                .Where(k => k.UserId == id && !k.Deleted);

            var user = ingredients.FirstOrDefault()?.User;

            return new Kitchen()
            {
                Ingredients = ingredients.ToArray(),
                User = user,
                UserId = user.Id,
            };
        }

        public Kitchen GetByName(string name)
        {
            var ingredients = context.Kitchens
                 .Include(k => k.Ingredient).ThenInclude(i => i.Categories)
                 .Include(k => k.Ingredient).ThenInclude(i => i.UnitTypes)
                 .Include(k => k.UnitType)
                 .Include(k => k.User)
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
