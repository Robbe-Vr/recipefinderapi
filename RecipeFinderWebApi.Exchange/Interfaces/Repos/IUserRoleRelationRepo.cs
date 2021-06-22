using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IUserRoleRelationRepo : IBaseRelationRepo<UserRoleRelation, User, Role>
    {
        public UserRoleRelation GetByUserIdAndRoleId(string userId, string roleId);
    }
}
