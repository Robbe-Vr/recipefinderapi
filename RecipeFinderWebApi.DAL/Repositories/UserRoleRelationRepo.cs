using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class UserRoleRelationRepo : AbstractRepo<UserRoleRelation>, IUserRoleRelationRepo
    {
        public UserRoleRelationRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<UserRoleRelation> GetAll()
        {
            return context.UserRoles
                .Include(x => x.User)
                .Include(x => x.Role)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public UserRoleRelation GetById(int id)
        {
            return context.UserRoles
                .Include(x => x.User)
                .Include(x => x.Role)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id);
        }

        public UserRoleRelation GetByUserIdAndRoleId(string userId, string roleId)
        {
            return context.UserRoles
                .Include(x => x.User)
                .Include(x => x.Role)
                .AsNoTracking()
                .FirstOrDefault(x => x.UserId == userId && x.RoleId == roleId);
        }

        public int CreateRelation(User user, Role role)
        {
            var relation = new UserRoleRelation()
            {
                UserId = user.Id,
                //User = user,
                RoleId = role.Id,
                //Role = role,
                Deleted = false,
            };

            context.UserRoles
                .Add(relation);

            return context.SaveChanges();
        }

        public int DeleteRelation(UserRoleRelation relation)
        {
            relation.User = null;
            relation.Role = null;

            context.UserRoles
                .Remove(relation);

            return context.SaveChanges();
        }
    }
}
