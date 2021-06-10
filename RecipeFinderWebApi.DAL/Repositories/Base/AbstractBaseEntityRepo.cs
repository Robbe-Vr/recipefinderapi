using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public abstract class AbstractBaseEntityRepo<TEntity> : AbstractBaseRepo<TEntity> where TEntity : class, ICountIdentifiedEntity
    {
        public AbstractBaseEntityRepo(RecipeFinderDbContext context, string table) : base(context, table) { }

        public abstract int Create(TEntity obj);
        public abstract int Update(TEntity obj);
        public abstract int Delete(TEntity obj);
    }
}
