using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public abstract class AbstractRepo<TEntity> where TEntity : class, ICountIdentifiedEntity
    {
        public RecipeFinderDbContext context;

        public AbstractRepo(RecipeFinderDbContext dbContext)
        {
            context = dbContext;
        }

        public bool KeyIsAttached(TEntity entity)
        {
            return context.Set<TEntity>().Local.Any(i => i.CountId == entity.CountId);
        }

        public TEntity GetAttachedEntityByEntity(TEntity entity)
        {
            return context.Set<TEntity>().Local.FirstOrDefault(i => i.CountId == entity.CountId);
        }

        public bool EntityIsAttached(TEntity entity)
        {
            return context.Set<TEntity>().Local.Any(i => i == entity);
        }

        public bool Exists(TEntity entity)
        {
            return context.Set<TEntity>().Any(i => i.CountId == entity.CountId);
        }
    }
}
