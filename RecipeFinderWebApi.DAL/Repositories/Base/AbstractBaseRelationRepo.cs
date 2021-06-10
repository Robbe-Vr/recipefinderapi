using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public abstract class AbstractBaseRelationRepo<TEntity, THost, TChild> : AbstractBaseRepo<TEntity> where TEntity : class, ICountIdentifiedEntity
    {
        public AbstractBaseRelationRepo(RecipeFinderDbContext context, string table) : base(context, table) { }

        public abstract int CreateRelation(TEntity obj);
        public abstract int CreateRelation(THost obj1, TChild obj2);

        public abstract int DeleteRelation(TEntity obj);
        public abstract int DeleteRelation(THost obj1, TChild obj2);
    }
}
