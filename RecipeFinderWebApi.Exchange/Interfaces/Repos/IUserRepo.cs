using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IUserRepo
    {
        public IEnumerable<User> GetAll();
        public IEnumerable<User> GetAllWithKitchen();

        public Kitchen GetKitchenById(string id);

        public User GetById(string id);
        public User GetByName(string name);

        public int Create(User user);

        public int Update(User user);

        public int Delete(User user);
    }
}
