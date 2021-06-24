using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
{
    public class UserHandler
    {
        private IUserRepo _repo;

        private IUserRoleRelationRepo _role_relation_repo;

        private IKitchenRepo _kitchen_repo;

        public UserHandler(IUserRepo repo, IUserRoleRelationRepo role_relation_repo, IKitchenRepo kitchen_repo)
        {
            _repo = repo;

            _role_relation_repo = role_relation_repo;

            _kitchen_repo = kitchen_repo;
        }

        public IEnumerable<User> GetAll()
        {
            return _repo.GetAll();
        }

        public IEnumerable<User> GetAllWithKitchen()
        {
            return _repo.GetAllWithKitchen();
        }

        public User GetById(string id)
        {
            return _repo.GetById(id);
        }

        public Kitchen GetKitchen(string id)
        {
            return _repo.GetKitchenById(id);
        }

        public User GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public IEnumerable<Role> GetRolesByUserId(string id)
        {
            return _repo.GetRolesByUserId(id);
        }

        public int Create(User user)
        {
            if (!Validate(user))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(user);

            if (validationResult != 0)
            {
                if (validationResult == -2)
                {
                    if (!_repo.TryRestore(user))
                    {
                        return 0;
                    }
                }

                return validationResult;
            }

            int changes = 0;

            List<Role> Roles = new List<Role>();
            Roles.AddRange(user.Roles);

            changes += _repo.Create(user);

            if (Roles.Count > 0)
            {
                foreach (Role role in Roles)
                {
                    changes += CreateRoleRelation(user, role);
                }
            }

            return changes;
        }

        public User CreateGetId(User user)
        {
            if (!Validate(user))
            {
                return null;
            }

            int validationResult = _repo.ValidateOriginality(user);

            if (validationResult != 0)
            {
                if (validationResult == -2)
                {
                    if (!_repo.TryRestore(user))
                    {
                        return null;
                    }
                }

                return null;
            }

            List<Role> Roles = new List<Role>();
            Roles.AddRange(user.Roles);

            user = _repo.CreateGetId(user);

            if (Roles.Count > 0)
            {
                foreach (Role role in Roles)
                {
                    CreateRoleRelation(user, role);
                }
            }

            return user;
        }

        public int Update(User user)
        {
            if (!Validate(user))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(user);

            if (validationResult != 0)
            {
                if (validationResult != -2)
                {
                    return validationResult;
                }
                else if (_repo.TryRestore(user))
                {
                    return validationResult;
                }
            }

            int changes = 0;

            var currentState = GetById(user.Id);

            user.CountId = currentState.CountId;

            List<Role> Roles = new List<Role>();
            Roles.AddRange(user.Roles);

            changes += _repo.Update(user);

            if (Roles.Count > 0)
            {
                IEnumerable<Role> toAddRoles = Roles.Where(x => !currentState.Roles.Contains(x));

                foreach (Role role in toAddRoles)
                {
                    changes += CreateRoleRelation(user, role);
                }

                IEnumerable<Role> toRemoveRoles = currentState.Roles.Where(x => !Roles.Contains(x));

                foreach (Role role in toRemoveRoles)
                {
                    changes += DeleteRoleRelation(user, role);
                }
            }

            return changes;
        }

        public int Delete(User user)
        {
            int changes = 0;

            var currentState = GetById(user.Id);

            var Kitchen = _kitchen_repo.GetByUserId(user.Id);
            var Roles = currentState.Roles;

            changes += _repo.Delete(currentState);

            foreach (Role role in Roles)
            {
                changes += DeleteRoleRelation(user, role);
            }

            foreach (KitchenIngredient ingredient in Kitchen.Ingredients)
            {
                changes += _kitchen_repo.Delete(ingredient);
            }

            return changes;
        }

        public int CreateRoleRelation(User user, Role role)
        {
            return _role_relation_repo.CreateRelation(user, role);
        }

        public int DeleteRoleRelation(UserRoleRelation relation)
        {
            return _role_relation_repo.DeleteRelation(relation);
        }

        public int DeleteRoleRelation(User user, Role role)
        {
            var relation = _role_relation_repo.GetByUserIdAndRoleId(user.Id, role.Id);

            return _role_relation_repo.DeleteRelation(relation);
        }

        private bool Validate(User user)
        {
            return (user.Name?.Length > 2 && user.NAME_NORMALIZED == user.Name?.ToUpper() &&
                user.Email?.Split('@').Length == 2 && user.Email?.Split('@')[1].Split('.').Length == 2 &&
                user.EMAIL_NORMALIZED == user.Email?.ToUpper() && user.EmailConfirmationToken?.Length > 0 &&
                user.ConcurrencyStamp?.Length > 0 && user.SecurityStamp?.Length > 0 &&
                user.PhoneNumber?.Length > 2 && user.AccessFailedCount >= 0 && user.Roles?.Count > 0 &&
                user.CreationDate < DateTime.Now && user.DOB < DateTime.Now.AddYears(-16) &&
                user.PasswordHashed?.Length > 8 && user.Salt?.Length > 2);
        }
    }
}
