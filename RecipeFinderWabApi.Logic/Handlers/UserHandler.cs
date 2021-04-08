using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
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
            int changes = 0;

            List<Role> Roles = new List<Role>();
            Roles.AddRange(user.Roles);

            Kitchen Kitchen = new Kitchen();
            Kitchen.Ingredients = user.Kitchen?.Ingredients;

            changes += _repo.Create(user);

            if (Roles.Count > 0)
            {
                foreach (Role role in Roles)
                {
                    changes += CreateRoleRelation(user, role);
                }
            }

            if (Kitchen?.Ingredients != null && Kitchen.Ingredients.Count > 0)
            {
                foreach (KitchenIngredient ingredient in user.Kitchen.Ingredients)
                {
                    changes += _kitchen_repo.Create(ingredient);
                }
            }

            return changes;
        }

        public User CreateGetId(User user)
        {
            List<Role> Roles = new List<Role>();
            Roles.AddRange(user.Roles);

            Kitchen Kitchen = new Kitchen();
            Kitchen.Ingredients = user.Kitchen?.Ingredients;

            user = _repo.CreateGetId(user);

            if (Roles.Count > 0)
            {
                foreach (Role role in Roles)
                {
                    CreateRoleRelation(user, role);
                }
            }

            if (Kitchen?.Ingredients != null && Kitchen.Ingredients.Count > 0)
            {
                foreach (KitchenIngredient ingredient in user.Kitchen.Ingredients)
                {
                    _kitchen_repo.Create(ingredient);
                }
            }

            return user;
        }

        public int Update(User user)
        {
            int changes = 0;

            var currentState = GetById(user.Id);

            user.CountId = currentState.CountId;

            List<Role> Roles = new List<Role>();
            Roles.AddRange(user.Roles);

            Kitchen Kitchen = new Kitchen();
            Kitchen.Ingredients = user.Kitchen?.Ingredients;

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

            if (Kitchen != null && Kitchen.Ingredients != null && Kitchen.Ingredients.Count > 0)
            {
                IEnumerable<KitchenIngredient> toAddIngredients = Kitchen.Ingredients.Where(x => !currentState.Kitchen.Ingredients.Contains(x));

                foreach (KitchenIngredient ingredient in toAddIngredients)
                {
                    changes += _kitchen_repo.Create(ingredient);
                }

                IEnumerable<KitchenIngredient> toRemoveIngredients = currentState.Kitchen.Ingredients.Where(x => !Kitchen.Ingredients.Contains(x));

                foreach (KitchenIngredient ingredient in toRemoveIngredients)
                {
                    changes += _kitchen_repo.Delete(ingredient);
                }

                IEnumerable<KitchenIngredient> toUpdateIngredients = Kitchen.Ingredients.Where(x => currentState.Kitchen.Ingredients.Any(y => x.IngredientId == y.IngredientId && (x.Units != y.Units || x.UnitTypeId != y.UnitTypeId)));

                foreach (KitchenIngredient ingredient in toUpdateIngredients)
                {
                    ingredient.UserId = currentState.Id;

                    ingredient.CountId = currentState.Kitchen.Ingredients.FirstOrDefault(i => i.IngredientId == ingredient.IngredientId).CountId;

                    changes += _kitchen_repo.Update(ingredient);
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

    }
}
