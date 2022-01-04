using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class UnitTypeRepo : AbstractBaseEntityRepo<UnitType>, IUnitTypeRepo
    {
        public UnitTypeRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.UnitTypes))
        {
        }

        public override IEnumerable<UnitType> GetAll()
        {
            return db
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public override UnitType GetById(int id)
        {
            return db
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .FirstOrDefault(x => (x.CountId == id) && !x.Deleted);
        }

        public UnitType GetByName(string name)
        {
            return db
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .FirstOrDefault(x => (x.Name == name) && !x.Deleted);
        }

        public override int Create(UnitType unitType)
        {
            unitType.CountId = 0;
            unitType.Ingredients = null;

            db.Add(unitType);

            return context.SaveChanges();
        }

        public override int Update(UnitType unitType)
        {
            unitType.Ingredients = null;

            if (!Exists(unitType))
            {
                return 0;
            }
            if (!EntityIsAttached(unitType))
            {
                if (KeyIsAttached(unitType))
                {
                    UnitType old = GetAttachedEntityByEntity(unitType);
                    old.Name = unitType.Name;
                }
                else db.Update(unitType);
            }

            return context.SaveChanges();
        }

        public override int Delete(UnitType unitType)
        {
            unitType.Ingredients = null;

            if (!Exists(unitType))
            {
                return 0;
            }
            if (!EntityIsAttached(unitType))
            {
                if (KeyIsAttached(unitType))
                {
                    unitType = GetAttachedEntityByEntity(unitType);
                }
            }

            context.Entry(unitType).State = EntityState.Modified;

            unitType.Deleted = true;

            return context.SaveChanges();
        }

        public override int ValidateOriginality(UnitType obj)
        {
            return db.Any(x => (x.Name == obj.Name) && !x.Deleted) ? -1 :
                db.Any(x => (x.Name == obj.Name) && x.Deleted) ? -2 :
                0;
        }

        public override bool TryRestore(UnitType obj)
        {
            UnitType restorable = db.FirstOrDefault(x => (x.Name == obj.Name) && x.Deleted);

            if (restorable == null) { return false; }

            db.Update(restorable);

            restorable.Deleted = false;

            context.SaveChanges();

            return true;
        }
    }
}
