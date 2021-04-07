using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class UnitTypeRepo : AbstractRepo<UnitType>, IUnitTypeRepo
    {
        public UnitTypeRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<UnitType> GetAll()
        {
            return context.UnitTypes
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public UnitType GetById(int id)
        {
            return context.UnitTypes
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id && !x.Deleted);
        }

        public UnitType GetByName(string name)
        {
            return context.UnitTypes
                .Include(x => x.Ingredients)
                .AsNoTracking()
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public int Create(UnitType unitType)
        {
            unitType.Ingredients = null;

            context.UnitTypes.Add(unitType);

            return context.SaveChanges();
        }

        public int Update(UnitType unitType)
        {
            context.UnitTypes.Update(unitType);

            return context.SaveChanges();
        }

        public int Delete(UnitType unitType)
        {
            unitType.Deleted = true;

            context.UnitTypes.Update(unitType);

            return context.SaveChanges();
        }
    }
}
