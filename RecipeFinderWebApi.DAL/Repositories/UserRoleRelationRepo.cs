using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class UserRoleRelationRepo : AbstractBaseRelationRepo<UserRoleRelation, User, Role>, IUserRoleRelationRepo
    {
        public UserRoleRelationRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.UserRoles))
        {
        }

        public override IEnumerable<UserRoleRelation> GetAll()
        {
            return db
                .Include(x => x.User)
                .Include(x => x.Role)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public override UserRoleRelation GetById(int id)
        {
            return db
                .Include(x => x.User)
                .Include(x => x.Role)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id);
        }

        public UserRoleRelation GetByUserIdAndRoleId(string userId, string roleId)
        {
            return db
                .Include(x => x.User)
                .Include(x => x.Role)
                .AsNoTracking()
                .FirstOrDefault(x => x.UserId == userId && x.RoleId == roleId);
        }

        public override int CreateRelation(UserRoleRelation relation)
        {
            db.Add(relation);

            return context.SaveChanges();
        }

        public override int CreateRelation(User user, Role role)
        {
            var relation = new UserRoleRelation()
            {
                UserId = user.Id,
                //User = user,
                RoleId = role.Id,
                //Role = role,
                Deleted = false,
            };

            db.Add(relation);

            return context.SaveChanges();
        }

        public override int DeleteRelation(UserRoleRelation relation)
        {
            if (!Exists(relation))
            {
                return 0;
            }
            if (!EntityIsAttached(relation))
            {
                if (KeyIsAttached(relation))
                {
                    relation = GetAttachedEntityByEntity(relation);
                }
            }

            relation.User = null;
            relation.Role = null;

            context.Entry(relation).State = EntityState.Deleted;

            return context.SaveChanges();
        }

        public override int DeleteRelation(User user, Role role)
        {
            var relation = new UserRoleRelation()
            {
                UserId = user.Id,
                //User = user,
                RoleId = role.Id,
                //Role = role,
                Deleted = false,
            };

            if (!Exists(relation))
            {
                return 0;
            }
            if (!EntityIsAttached(relation))
            {
                if (KeyIsAttached(relation))
                {
                    relation = GetAttachedEntityByEntity(relation);
                }
            }

            relation.User = null;
            relation.Role = null;

            context.Entry(relation).State = EntityState.Deleted;

            return context.SaveChanges();
        }
    }
}
