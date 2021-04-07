using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IKitchenRepo
    {
        public IEnumerable<KitchenIngredient> GetAll();

        public KitchenIngredient GetById(int id);

        public Kitchen GetByUserId(string id);

        public Kitchen GetByUserName(string name);

        public int Create(KitchenIngredient ingredient);

        public int Update(KitchenIngredient ingredient);

        public int Delete(KitchenIngredient ingredient);
    }
}
