using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
{
    public class RoleHandler
    {
        private IRoleRepo _repo;

        public RoleHandler(IRoleRepo repo)
        {
            _repo = repo;
        }

        public IEnumerable<Role> GetAll()
        {
            return _repo.GetAll();
        }

        public Role GetById(string id)
        {
            return _repo.GetById(id);
        }

        public Role GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(Role user)
        {
            return _repo.Create(user);
        }

        public int Update(Role user)
        {
            return _repo.Update(user);
        }

        public int Delete(Role user)
        {
            return _repo.Delete(user);
        }
    }
}
