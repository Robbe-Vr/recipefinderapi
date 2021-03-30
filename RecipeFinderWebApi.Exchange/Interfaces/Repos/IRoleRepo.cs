using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IRoleRepo
    {
        public IEnumerable<Role> GetAll();

        public Role GetById(string id);
        public Role GetByName(string name);

        public int Create(Role user);

        public int Update(Role user);

        public int Delete(Role user);
    }
}
