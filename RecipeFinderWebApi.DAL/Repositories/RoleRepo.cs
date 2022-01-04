using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class RoleRepo : AbstractBaseEntityRepo<Role>, IRoleRepo
    {
        public RoleRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.Roles))
        {
        }

        public override IEnumerable<Role> GetAll()
        {
            return db
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public override Role GetById(int id)
        {
            return db
                .AsNoTracking()
                .FirstOrDefault(x => (x.CountId == id) && !x.Deleted);
        }

        public Role GetById(string id)
        {
            return db
                .AsNoTracking()
                .FirstOrDefault(x => (x.Id == id) && !x.Deleted);
        }

        public Role GetByName(string name)
        {
            return db
                .AsNoTracking()
                .FirstOrDefault(x => (x.Name == name) && !x.Deleted);
        }

        public override int Create(Role role)
        {
            role.CountId = 0;
            role.Users = null;

            role.Id = Guid.NewGuid().ToString();

            db.Add(role);

            return context.SaveChanges();
        }

        public override int Update(Role role)
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
                else db.Update(role);
            }

            return context.SaveChanges();
        }

        public override int Delete(Role role)
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

        public override int ValidateOriginality(Role obj)
        {
            return db.Any(x => (x.Name == obj.Name) && !x.Deleted) ? -1 :
                db.Any(x => (x.Name == obj.Name) && x.Deleted) ? -2 :
                0;
        }

        public override bool TryRestore(Role obj)
        {
            Role restorable = db.FirstOrDefault(x => (x.Name == obj.Name) && x.Deleted);

            if (restorable == null) { return false; }

            db.Update(restorable);

            restorable.Deleted = false;

            context.SaveChanges();

            return true;
        }
    }
}
