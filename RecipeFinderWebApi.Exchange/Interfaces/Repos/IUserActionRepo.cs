using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IUserActionRepo
    {
        public IEnumerable<UserAction> GetAll();
        public UserAction GetById(int id);

        public IEnumerable<UserAction> GetAllByUser(int userId);
        public int Create(UserAction action);
        public int Update(UserAction action);
        public int Delete(UserAction action);
    }
}
