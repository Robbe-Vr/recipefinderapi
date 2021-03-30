using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class UnitTypeRepo : IUnitTypeRepo
    {
        private RecipeFinderDBContext context = new RecipeFinderDBContext(RecipeFinderDBContext.ops.dbOptions);

        public IEnumerable<UnitType> GetAll()
        {
            return context.UnitTypes
                .Include(x => x.Ingredients)
                .Where(x => !x.Deleted);
        }

        public UnitType GetById(int id)
        {
            return context.UnitTypes
                .Include(x => x.Ingredients)
                .FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public UnitType GetByName(string name)
        {
            return context.UnitTypes
                .Include(x => x.Ingredients)
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public int Create(UnitType ingredient)
        {
            context.UnitTypes.Add(ingredient);

            return context.SaveChanges();
        }

        public int Update(UnitType ingredient)
        {
            context.UnitTypes.Update(ingredient);

            return context.SaveChanges();
        }

        public int Delete(UnitType ingredient)
        {
            ingredient.Deleted = true;

            context.UnitTypes.Update(ingredient);

            return context.SaveChanges();
        }
    }
}
