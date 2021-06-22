using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IBaseEntityRepo<TEntity> : IBaseRepo<TEntity> where TEntity : class
    {
        public int Create(TEntity obj);
        public int Update(TEntity obj);
        public int Delete(TEntity obj);
    }
}
