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
            role.Users = null;

            if (!Exists(role))
            {
                return 0;
            }
            if (!EntityIsAttached(role))
            {
                if (KeyIsAttached(role))
                {
                    Role old = GetAttachedEntityByEntity(role);
                    old.Name = role.Name;
                }
                else context.Roles.Update(role);
            }

            return context.SaveChanges();
        }

        public int Delete(Role role)
        {
            role.Users = null;

            if (!Exists(role))
            {
                return 0;
            }
            if (!EntityIsAttached(role))
            {
                if (KeyIsAttached(role))
                {
                    role = GetAttachedEntityByEntity(role);
                }
            }

            context.Entry(role).State = EntityState.Modified;

            role.Deleted = true;

            return context.SaveChanges();
        }
    }
}
