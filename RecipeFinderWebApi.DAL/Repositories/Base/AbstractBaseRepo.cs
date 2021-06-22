using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public abstract class AbstractBaseRepo<TEntity> where TEntity : class, ICountIdentifiedEntity
    {
        public RecipeFinderDbContext context;
        public DbSet<TEntity> db;

        public AbstractBaseRepo(RecipeFinderDbContext dbContext, string table)
        {
            context = dbContext;
            db = context.GetTable<TEntity>(table);
        }

        public abstract IEnumerable<TEntity> GetAll();
        public abstract TEntity GetById(int id);
        

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

        /// <summary>
        /// returns 0 if originality is valid,
        /// returns -1 if an entity with equal property values already exists,
        /// returns -2 if a deleted entity with equal property values exists,
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract int ValidateOriginality(TEntity obj);

        public abstract bool TryRestore(TEntity obj);
    }
}
