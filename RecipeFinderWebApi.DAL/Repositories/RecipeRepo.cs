using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class RecipeRepo : AbstractRepo<Recipe>, IRecipeRepo
    {
        private User currentUser;

        public RecipeRepo(RecipeFinderDbContext dbContext, User currentUser) : base(dbContext)
        {
            this.currentUser = currentUser;
        }

        public IEnumerable<Recipe> GetAll()
        {
            return context.Recipes
                .Include(x => x.Categories)
                .Include(x => x.User)
                .AsNoTracking()
                .Where(x => (x.IsPublic || x.UserId == currentUser.Id) && !x.Deleted);
        }

        public IEnumerable<Recipe> GetAllByCook(string userId)
        {
            return context.Recipes
                .Include(x => x.Categories)
                .Include(x => x.User)
                .AsNoTracking()
                .Where(x => x.UserId == userId && (x.IsPublic || x.UserId == currentUser.Id) && !x.Deleted);
        }

        public Recipe GetById(string id)
        {
            return context.Recipes
                .Include(x => x.Categories)
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id && (x.IsPublic || x.UserId == currentUser.Id) && !x.Deleted);
        }

        public Recipe GetByName(string name)
        {
            return context.Recipes
                .Include(x => x.Categories)
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefault(x => x.Name == name && (x.IsPublic || x.UserId == currentUser.Id) && !x.Deleted);
        }

        public int Create(Recipe recipe)
        {
            recipe.User = null;
            recipe.Categories = null;

            recipe.Id = Guid.NewGuid().ToString();

            context.Recipes.Add(recipe);

            return context.SaveChanges();
        }

        public int Update(Recipe recipe)
        {
            recipe.User = null;
            recipe.Categories = null;

            if (!Exists(recipe))
            {
                return 0;
            }
            if (!EntityIsAttached(recipe))
            {
                if (KeyIsAttached(recipe))
                {
                    Recipe old = GetAttachedEntityByEntity(recipe);

                    old.Name = recipe.Name;
                    old.PreparationSteps = recipe.PreparationSteps;
                    old.VideoTutorialLink = recipe.VideoTutorialLink;
                    old.Description = recipe.Description;
                }
                else context.Recipes.Update(recipe);
            }

            return context.SaveChanges();
        }

        public int Delete(Recipe recipe)
        {
            recipe.User = null;
            recipe.Categories = null;

            if (!Exists(recipe))
            {
                return 0;
            }
            if (!EntityIsAttached(recipe))
            {
                if (KeyIsAttached(recipe))
                {
                    recipe = GetAttachedEntityByEntity(recipe);
                }
            }

            context.Entry(recipe).State = EntityState.Modified;

            recipe.Deleted = true;

            return context.SaveChanges();
        }
    }
}
