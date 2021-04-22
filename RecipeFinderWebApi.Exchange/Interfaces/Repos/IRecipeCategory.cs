using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IRecipeCategoryRepo
    {
        public IEnumerable<RecipeCategory> GetAll();

        public RecipeCategory GetById(int id);
        public RecipeCategory GetByName(string name);

        public int Create(RecipeCategory category);

        public int Update(RecipeCategory category);

        public int Delete(RecipeCategory category);
    }
}
