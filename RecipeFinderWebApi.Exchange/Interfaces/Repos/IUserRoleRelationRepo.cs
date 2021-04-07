using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IUserRoleRelationRepo
    {
        public IEnumerable<UserRoleRelation> GetAll();

        public UserRoleRelation GetById(int id);

        public UserRoleRelation GetByUserIdAndRoleId(string userId, string roleId);

        public int CreateRelation(User user, Role role);

        public int DeleteRelation(UserRoleRelation relation);
    }
}
