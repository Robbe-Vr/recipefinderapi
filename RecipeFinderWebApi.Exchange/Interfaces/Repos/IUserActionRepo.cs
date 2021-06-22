using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IUserActionRepo : IBaseEntityRepo<UserAction>
    {
        public IEnumerable<UserAction> GetAllByUser(int userId);
    }
}
