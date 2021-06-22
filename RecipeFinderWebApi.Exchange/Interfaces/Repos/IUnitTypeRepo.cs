using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IUnitTypeRepo : IBaseEntityRepo<UnitType>
    {
        public UnitType GetByName(string name);
    }
}
