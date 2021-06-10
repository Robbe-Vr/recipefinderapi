using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class UserActionRepo : AbstractBaseEntityRepo<UserAction>, IUserActionRepo
    {
        public UserActionRepo(RecipeFinderDbContext context) : base(context, nameof(RecipeFinderDbContext.UserActions))
        {
        }

        public override IEnumerable<UserAction> GetAll()
        {
            return db
                .Include(x => x.User)
                .AsNoTracking();
        }

        public override UserAction GetById(int id)
        {
            return db
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefault(x => x.CountId == id);
        }

        public IEnumerable<UserAction> GetAllByUser(int userId)
        {
            return db
                .Include(x => x.User)
                .AsNoTracking()
                .Where(x => x.UserId == userId);
        }

        public override int Create(UserAction action)
        {
            action.User = null;

            db.Add(action);

            return context.SaveChanges();
        }

        public override int Update(UserAction action)
        {
            action.User = null;

            if (!Exists(action))
            {
                return 0;
            }
            if (!EntityIsAttached(action))
            {
                if (KeyIsAttached(action))
                {
                    UserAction old = GetAttachedEntityByEntity(action);

                    old.ActionPerformedOnTable = action.ActionPerformedOnTable;
                    old.RefObjectId = action.RefObjectId;
                    old.RefObjectName = action.RefObjectName;
                    old.UserId = action.UserId;
                    old.Description = action.Description;
                }
                else db.Update(action);
            }

            return context.SaveChanges();
        }

        public override int Delete(UserAction action)
        {
            if (!Exists(action))
            {
                return 0;
            }
            if (!EntityIsAttached(action))
            {
                if (KeyIsAttached(action))
                {
                    action = GetAttachedEntityByEntity(action);
                }
            }

            action.User = null;

            context.Entry(action).State = EntityState.Deleted;

            return context.SaveChanges();
        }
    }
}
