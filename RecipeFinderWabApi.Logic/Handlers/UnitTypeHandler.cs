using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
{
    public class UnitTypeHandler
    {
        private IUnitTypeRepo _repo;

        public UnitTypeHandler(IUnitTypeRepo repo)
        {
            _repo = repo;
        }

        public IEnumerable<UnitType> GetAll()
        {
            return _repo.GetAll();
        }

        public UnitType GetById(int id)
        {
            return _repo.GetById(id);
        }

        public UnitType GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(UnitType ingredient)
        {
            return _repo.Create(ingredient);
        }

        public int Update(UnitType ingredient)
        {
            return _repo.Update(ingredient);
        }

        public int Delete(UnitType ingredient)
        {
            return _repo.Delete(ingredient);
        }
    }
}
