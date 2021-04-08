using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class UserRepo : AbstractRepo<User>, IUserRepo
    {
        public UserRepo(RecipeFinderDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<User> GetAll()
        {
            return context.Users
                .Include(x => x.Roles)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public IEnumerable<User> GetAllWithKitchen()
        {
            return context.Users
                .Include(x => x.Kitchen.Ingredients)
                    .ThenInclude(x => x.Ingredient)
                .Include(x => x.Kitchen.Ingredients)
                    .ThenInclude(x => x.UnitType)
                .Include(x => x.Roles)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public Kitchen GetKitchenById(string id)
        {
            return context.Users
                .Include(x => x.Kitchen.Ingredients)
                    .ThenInclude(x => x.Ingredient)
                .Include(x => x.Kitchen.Ingredients)
                    .ThenInclude(x => x.UnitType)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id && !x.Deleted).Kitchen;
        }

        public User GetById(string id)
        {
            return context.Users
                .Include(x => x.Roles)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public User GetByName(string name)
        {
            return context.Users
                .Include(x => x.Roles)
                .AsNoTracking()
                .FirstOrDefault(x => x.Name == name && !x.Deleted);
        }

        public IEnumerable<Role> GetRolesByUserId(string id)
        {
            return context.Users
                .Include(x => x.Roles)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id && !x.Deleted)
                .Roles;
        }

        public int Create(User user)
        {
            user.Roles = null;
            user.Kitchen = null;

            user.Id = Guid.NewGuid().ToString();

            context.Users.Add(user);

            return context.SaveChanges();
        }

        public User CreateGetId(User user)
        {
            user.Roles = null;
            user.Kitchen = null;

            user.Id = Guid.NewGuid().ToString();

            context.Users.Add(user);

           context.SaveChanges();

            return user;
        }

        public int Update(User user)
        {
            user.Roles = null;
            user.Kitchen = null;

            if (!Exists(user))
            {
                return 0;
            }
            if (!EntityIsAttached(user))
            {
                if (KeyIsAttached(user))
                {
                    User old = GetAttachedEntityByEntity(user);

                    old.Name = user.Name;
                    old.NAME_NORMALIZED = user.NAME_NORMALIZED;
                    old.Email = user.Email;
                    old.EMAIL_NORMALIZED = user.EMAIL_NORMALIZED;
                    old.EmailConfirmed = user.EmailConfirmed;
                    old.EmailConfirmationToken = user.EmailConfirmationToken;
                    old.DOB = user.DOB;
                    old.ConcurrencyStamp = user.ConcurrencyStamp;
                    old.SecurityStamp = user.SecurityStamp;
                    old.PasswordHashed = user.PasswordHashed;
                    old.PhoneNumber = user.PhoneNumber;
                    old.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                    old.LockoutEnabled = user.LockoutEnabled;
                    old.LockoutEnd = user.LockoutEnd;
                    old.AccessFailedCount = user.AccessFailedCount;
                    old.Deleted = user.Deleted;
                }
                else context.Users.Update(user);
            }

            return context.SaveChanges();
        }

        public int Delete(User user)
        {
            user.Roles = null;
            user.Kitchen = null;

            if (!Exists(user))
            {
                return 0;
            }
            if (!EntityIsAttached(user))
            {
                if (KeyIsAttached(user))
                {
                    user = GetAttachedEntityByEntity(user);
                }
                else context.Users.Update(user);
            }

            user.Deleted = true;

            return context.SaveChanges();
        }
    }
}
