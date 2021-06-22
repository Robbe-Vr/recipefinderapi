using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class RecipeRepo : AbstractBaseEntityRepo<Recipe>, IRecipeRepo
    {
        private User currentUser;

        public RecipeRepo(RecipeFinderDbContext dbContext, User currentUser) : base(dbContext, nameof(RecipeFinderDbContext.Recipes))
        {
            this.currentUser = currentUser;
        }

        public override IEnumerable<Recipe> GetAll()
        {
            return db
                .Include(x => x.Categories)
                .Include(x => x.User)
                .AsNoTracking()
                .Where(x => (x.IsPublic || x.UserId == currentUser.Id) && !x.Deleted);
        }

        public IEnumerable<Recipe> GetAllByCook(string userId)
        {
            return db
                .Include(x => x.Categories)
                .Include(x => x.User)
                .AsNoTracking()
                .Where(x => x.UserId == userId && (x.IsPublic || x.UserId == currentUser.Id) && !x.Deleted);
        }

        public override Recipe GetById(int id)
        {
            return db
                .Include(x => x.Categories)
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id && (x.IsPublic || x.UserId == currentUser.Id) && !x.Deleted);
        }

        public Recipe GetById(string id)
        {
            return db
                .Include(x => x.Categories)
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id && (x.IsPublic || x.UserId == currentUser.Id) && !x.Deleted);
        }

        public Recipe GetByName(string name)
        {
            return db
                .Include(x => x.Categories)
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefault(x => x.Name == name && (x.IsPublic || x.UserId == currentUser.Id) && !x.Deleted);
        }

        public override int Create(Recipe recipe)
        {
            recipe.CountId = 0;
            recipe.User = null;
            recipe.Categories = null;

            recipe.Id = Guid.NewGuid().ToString();

            db.Add(recipe);

            return context.SaveChanges();
        }

        public override int Update(Recipe recipe)
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
                else db.Update(recipe);
            }

            return context.SaveChanges();
        }

        public override int Delete(Recipe recipe)
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

        public override int ValidateOriginality(Recipe obj)
        {
            return db.Any(x => x.Name == obj.Name && x.UserId == obj.UserId && !x.Deleted) ? -1 :
                db.Any(x => x.Name == obj.Name && x.UserId == obj.UserId && x.Deleted) ? -2 :
                0;
        }

        public override bool TryRestore(Recipe obj)
        {
            Recipe restorable = db.FirstOrDefault(x => x.Name == obj.Name && x.UserId == obj.UserId && x.Deleted);

            if (restorable == null) { return false; }

            db.Update(restorable);

            restorable.Deleted = false;

            context.SaveChanges();

            return true;
        }
    }
}
