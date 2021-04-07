using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class IngredientCategoryRelationRepo : AbstractRepo<IngredientCategoryRelation>, IIngredientCategoryRelationRepo
    {
        public IngredientCategoryRelationRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<IngredientCategoryRelation> GetAll()
        {
            return context.CategoriesIngredient
                .Include(x => x.Ingredient)
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public IngredientCategoryRelation GetById(int id)
        {
            return context.CategoriesIngredient
                .Include(x => x.Ingredient)
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id);
        }

        public IngredientCategoryRelation GetByIngredientIdAndCategoryId(string ingredientId, int categoryId)
        {
            return context.CategoriesIngredient
                .Include(x => x.Ingredient)
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefault(x => x.IngredientId == ingredientId && x.CategoryId == categoryId);
        }

        public int CreateRelation(Ingredient ingredient, IngredientCategory category)
        {
            var relation = new IngredientCategoryRelation()
            {
                IngredientId = ingredient.Id,
                //Ingredient = ingredient,
                CategoryId = category.CountId,
                //Category = category,
                Deleted = false,
            };

            context.CategoriesIngredient
                .Add(relation);

            return context.SaveChanges();
        }

        public int DeleteRelation(IngredientCategoryRelation relation)
        {
            context.CategoriesIngredient
                .Remove(relation);

            return context.SaveChanges();
        }
    }
}
