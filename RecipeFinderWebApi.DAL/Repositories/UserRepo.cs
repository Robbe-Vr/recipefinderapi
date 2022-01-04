using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.DAL.Repositories
{
    public class UserRepo : AbstractBaseEntityRepo<User>, IUserRepo
    {
        public UserRepo(RecipeFinderDbContext dbContext) : base(dbContext, nameof(RecipeFinderDbContext.Users))
        {
        }

        public override IEnumerable<User> GetAll()
        {
            return db
                .Include(x => x.Roles)
                .AsNoTracking()
                .Where(x => !x.Deleted);
        }

        public IEnumerable<UserWithKitchen> GetAllWithKitchen()
        {
            IEnumerable<UserWithKitchen> users = (IEnumerable<UserWithKitchen>)db
                .Include(x => x.Roles)
                .AsNoTracking()
                .Where(x => !x.Deleted);

            foreach (UserWithKitchen user in users)
            {
                user.Kitchen = GetKitchenById(user.Id);

                user.Kitchen.User = user;
            }

            return users;
        }

        public Kitchen GetKitchenById(string id)
        {
            return new Kitchen()
            {
                Ingredients = context.Kitchens
                    .AsNoTracking()
                    .Where(x => (x.UserId == id) && !x.Deleted).ToList(),
                UserId = id,
            };
        }

        public override User GetById(int id)
        {
            return db
                .Include(x => x.Roles)
                .AsNoTracking()
                .FirstOrDefault(x => (x.CountId == id) && !x.Deleted);
        }

        public User GetById(string id)
        {
            return db
                .Include(x => x.Roles)
                .AsNoTracking()
                .FirstOrDefault(x => (x.Id == id) && !x.Deleted);
        }

        public User GetByName(string name)
        {
            return db
                .Include(x => x.Roles)
                .AsNoTracking()
                .FirstOrDefault(x => (x.Name == name || x.EMAIL_NORMALIZED == name.ToUpper()) && !x.Deleted);
        }

        public IEnumerable<Role> GetRolesByUserId(string id)
        {
            return db
                .Include(x => x.Roles)
                .AsNoTracking()
                .FirstOrDefault(x => (x.Id == id) && !x.Deleted)
                .Roles;
        }

        public override int Create(User user)
        {
            user.CountId = 0;
            user.Roles = null;

            user.Id = Guid.NewGuid().ToString();

            db.Add(user);

            return context.SaveChanges();
        }

        public User CreateGetId(User user)
        {
            user.Roles = null;

            user.Id = Guid.NewGuid().ToString();

            db.Add(user);

           context.SaveChanges();

            return user;
        }

        public override int Update(User user)
        {
            user.Roles = null;

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
                else db.Update(user);
            }

            return context.SaveChanges();
        }

        public override int Delete(User user)
        {
            user.Roles = null;

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
                else db.Update(user);
            }

            user.Deleted = true;

            return context.SaveChanges();
        }

        /// <summary>
        /// returns 0 if originality is valid,
        /// returns -1 if an user with the same name already exists,
        /// returns -2 if an user with the same email already exists,
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int ValidateOriginality(User obj)
        {
            return db.Any(x => x.Name == obj.Name && x.CountId != obj.CountId) ? -3 :
                db.Any(x => x.EMAIL_NORMALIZED == obj.EMAIL_NORMALIZED && x.CountId != obj.CountId) ? -4 :
                0;
        }

        public override bool TryRestore(User obj)
        {
            User restorable = db.FirstOrDefault(x => (x.Name == obj.Name || x.EMAIL_NORMALIZED == obj.EMAIL_NORMALIZED) && x.Deleted);

            if (restorable == null) { return false; }

            db.Update(restorable);

            restorable.Deleted = false;

            context.SaveChanges();

            return true;
        }
    }
}
