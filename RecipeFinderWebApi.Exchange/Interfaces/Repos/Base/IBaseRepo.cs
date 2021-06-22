using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IBaseRepo<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> GetAll();
        public TEntity GetById(int id);

        public int ValidateOriginality(TEntity obj);

        public bool TryRestore(TEntity obj);
    }
}
