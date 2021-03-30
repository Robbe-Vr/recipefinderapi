using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class RoleRepo : IRoleRepo
    {
        private RecipeFinderDBContext context = new RecipeFinderDBContext(RecipeFinderDBContext.ops.dbOptions);

        public IEnumerable<Role> GetAll()
        {
            return context.Roles
                .Where(x => !x.Deleted);
        }

        public Role GetById(string id)
        {
            return context.Roles
                .FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public Role GetByName(string name)
        {
            return context.Roles
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public int Create(Role user)
        {
            context.Roles.Add(user);

            return context.SaveChanges();
        }

        public int Update(Role user)
        {
            context.Roles.Update(user);

            return context.SaveChanges();
        }

        public int Delete(Role user)
        {
            user.Deleted = true;

            context.Roles.Update(user);

            return context.SaveChanges();
        }
    }
}
