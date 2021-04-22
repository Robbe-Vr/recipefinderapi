using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class RecipeCategoryRelationRepo : AbstractRepo<RecipeCategoryRelation>, IRecipeCategoryRelationRepo
    {
        public RecipeCategoryRelationRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<RecipeCategoryRelation> GetAll()
        {
            return context.CategoriesRecipe
                .Include(x => x.Recipe)
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public RecipeCategoryRelation GetById(int id)
        {
            return context.CategoriesRecipe
                .Include(x => x.Recipe)
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id);
        }

        public RecipeCategoryRelation GetByRecipeIdAndCategoryId(string recipeId, int categoryId)
        {
            return context.CategoriesRecipe
                .Include(x => x.Recipe)
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefault(x => x.RecipeId == recipeId && x.CategoryId == categoryId);
        }

        public int CreateRelation(Recipe recipe, RecipeCategory category)
        {
            var relation = new RecipeCategoryRelation()
            {
                RecipeId = recipe.Id,
                //Recipe = recipe,
                CategoryId = category.CountId,
                //Category = category,
                Deleted = false,
            };

            context.CategoriesRecipe
                .Add(relation);

            return context.SaveChanges();
        }

        public int DeleteRelation(RecipeCategoryRelation relation)
        {
            if (!Exists(relation))
            {
                return 0;
            }
            if (!EntityIsAttached(relation))
            {
                if (KeyIsAttached(relation))
                {
                    relation = GetAttachedEntityByEntity(relation);
                }
            }

            relation.Recipe = null;
            relation.Category = null;

            context.Entry(relation).State = EntityState.Deleted;

            return context.SaveChanges();
        }
    }
}
