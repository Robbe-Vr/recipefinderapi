using System;
using System.Collections.Generic;
using System.Text;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IRoleRepo : IBaseEntityRepo<Role>
    {
        public Role GetById(string id);
        public Role GetByName(string name);
    }
}
