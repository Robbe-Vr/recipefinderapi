﻿using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
{
    public class RoleHandler
    {
        private IRoleRepo _repo;

        private IUserRoleRelationRepo _user_relation_repo;

        public RoleHandler(IRoleRepo repo, IUserRoleRelationRepo user_relation_repo)
        {
            _repo = repo;

            _user_relation_repo = user_relation_repo;
        }

        public IEnumerable<Role> GetAll()
        {
            return _repo.GetAll();
        }

        public Role GetById(string id)
        {
            return _repo.GetById(id);
        }

        public Role GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(Role role)
        {
            if (!Validate(role))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(role);

            if (validationResult != 0)
            {
                if (validationResult == -2)
                {
                    if (!_repo.TryRestore(role))
                    {
                        return 0;
                    }
                }

                return validationResult;
            }

            return _repo.Create(role);
        }

        public int CreateUserRelation(User user, Role role)
        {
            return _user_relation_repo.CreateRelation(user, role);
        }

        public int DeleteUserRelation(UserRoleRelation relation)
        {
            return _user_relation_repo.DeleteRelation(relation);
        }

        public int DeleteUserRelation(User user, Role role)
        {
            var relation = _user_relation_repo.GetByUserIdAndRoleId(user.Id, role.Id);

            return _user_relation_repo.DeleteRelation(relation);
        }

        public int Update(Role role)
        {
            if (!Validate(role))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(role);

            if (validationResult != 0)
            {
                if (validationResult != -2)
                {
                    return validationResult;
                }
                else if (_repo.TryRestore(role))
                {
                    return validationResult;
                }
            }

            return _repo.Update(role);
        }

        public int Delete(Role role)
        {
            return _repo.Delete(GetById(role.Id));
        }

        private bool Validate(Role role)
        {
            return (role.Name?.Length > 2);
        }
    }
}
