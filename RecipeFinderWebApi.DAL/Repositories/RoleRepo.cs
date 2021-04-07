using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class RoleRepo : AbstractRepo<Role>, IRoleRepo
    {
        public RoleRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Role> GetAll()
        {
            return context.Roles
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public Role GetById(string id)
        {
            return context.Roles
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public Role GetByName(string name)
        {
            return context.Roles
                .AsNoTracking()
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public int Create(Role role)
        {
            role.Users = null;

            role.Id = Guid.NewGuid().ToString();

            context.Roles.Add(role);

            return context.SaveChanges();
        }

        public int Update(Role role)
        {
            context.Roles.Update(role);

            return context.SaveChanges();
        }

        public int Delete(Role role)
        {
            role.Deleted = true;

            context.Roles.Update(role);

            return context.SaveChanges();
        }
    }
}
