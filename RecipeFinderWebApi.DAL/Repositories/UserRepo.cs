using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class UserRepo : IUserRepo
    {
        private RecipeFinderDBContext context = new RecipeFinderDBContext(RecipeFinderDBContext.ops.dbOptions);

        public IEnumerable<User> GetAll()
        {
            return context.Users
                .Include(x => x.Roles)
                .Where(x => !x.Deleted);
        }

        public IEnumerable<User> GetAllWithKitchen()
        {
            return context.Users
                .Include(x => x.Kitchen.Ingredients)
                    .ThenInclude(x => x.Ingredient)
                .Include(x => x.Kitchen.Ingredients)
                    .ThenInclude(x => x.UnitType)
                .Include(x => x.Roles)
                .Where(x => !x.Deleted);
        }

        public Kitchen GetKitchenById(string id)
        {
            return context.Users
                .Include(x => x.Kitchen.Ingredients)
                    .ThenInclude(x => x.Ingredient)
                .Include(x => x.Kitchen.Ingredients)
                    .ThenInclude(x => x.UnitType)
                .FirstOrDefault(x => x.Id == id && !x.Deleted).Kitchen;
        }

        public User GetById(string id)
        {
            return context.Users
                .Include(x => x.Roles)
                .FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public User GetByName(string name)
        {
            return context.Users
                .Include(x => x.Roles)
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public int Create(User user)
        {
            context.Users.Add(user);

            return context.SaveChanges();
        }

        public int Update(User user)
        {
            context.Users.Update(user);

            return context.SaveChanges();
        }

        public int Delete(User user)
        {
            user.Deleted = true;

            context.Users.Update(user);

            return context.SaveChanges();
        }
    }
}
