using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IUnitTypeRepo
    {
        public IEnumerable<UnitType> GetAll();

        public UnitType GetById(int id);
        public UnitType GetByName(string name);

        public int Create(UnitType unitType);

        public int Update(UnitType unitType);

        public int Delete(UnitType unitType);
    }
}
