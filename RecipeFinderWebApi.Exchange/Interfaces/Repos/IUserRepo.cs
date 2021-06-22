using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IUserRepo : IBaseEntityRepo<User>
    {
        public IEnumerable<UserWithKitchen> GetAllWithKitchen();

        public Kitchen GetKitchenById(string id);

        public User GetById(string id);

        public User GetByName(string name);

        public IEnumerable<Role> GetRolesByUserId(string id);

        User CreateGetId(User user);
    }
}
