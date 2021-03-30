using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IKitchenRepo
    {
        public IEnumerable<KitchenIngredient> GetAll();

        public Kitchen GetById(string id);
        public Kitchen GetByName(string name);

        public int Create(KitchenIngredient ingredient);

        public int Update(KitchenIngredient ingredient);

        public int Delete(KitchenIngredient ingredient);
    }
}
