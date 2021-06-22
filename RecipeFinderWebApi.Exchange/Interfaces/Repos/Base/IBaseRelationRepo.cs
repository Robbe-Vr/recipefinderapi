using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.Exchange.Interfaces.Repos
{
    public interface IBaseRelationRepo<TEntity, THost, TChild> : IBaseRepo<TEntity> where TEntity : class
    {
        public int CreateRelation(TEntity obj);
        public int CreateRelation(THost obj1, TChild obj2);

        public int DeleteRelation(TEntity obj);
        public int DeleteRelation(THost obj1, TChild obj2);
    }
}
