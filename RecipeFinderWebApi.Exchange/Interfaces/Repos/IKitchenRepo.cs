using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IKitchenRepo : IBaseEntityRepo<KitchenIngredient>
    {
        public Kitchen GetByUserId(string id);

        public Kitchen GetByUserName(string name);
    }
}
