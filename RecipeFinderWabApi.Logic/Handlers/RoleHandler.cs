﻿using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
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

        public int Create(Role user)
        {
            return _repo.Create(user);
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

        public int Update(Role user)
        {
            return _repo.Update(user);
        }

        public int Delete(Role user)
        {
            return _repo.Delete(user);
        }
    }
}
