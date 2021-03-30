using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IRequirementsListRepo
    {
        public IEnumerable<RequirementsListIngredient> GetAll();

        public RequirementsList GetById(string id);
        public RequirementsList GetByName(string name);

        public int Create(RequirementsListIngredient ingredient);

        public int Update(RequirementsListIngredient ingredient);

        public int Delete(RequirementsListIngredient ingredient);
    }
}
