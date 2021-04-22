﻿using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
{
    public class UserActionHandler
    {
        private IUserActionRepo _repo;
        private IUserRepo _user_repo;

        public UserActionHandler(IUserActionRepo repo, IUserRepo user_repo)
        {
            _repo = repo;
            _user_repo = user_repo;
        }

        public IEnumerable<UserAction> GetAll()
        {
            return _repo.GetAll();
        }

        public UserAction GetById(int id)
        {
            return _repo.GetById(id);
        }

        public IEnumerable<UserAction> GetAllByUserId(string id)
        {
            User user = _user_repo.GetById(id);

            if (user != null)
            {
                return _repo.GetAllByUser(user.CountId);
            }

            return new List<UserAction>();
        }

        public IEnumerable<UserAction> GetAllByUserId(int  id)
        {
            return _repo.GetAllByUser(id);
        }

        public int Create(UserAction action)
        {
            return _repo.Create(action);
        }

        public int Update(UserAction action)
        {
            return _repo.Update(action);
        }

        public int Delete(UserAction action)
        {
            return _repo.Delete(action);
        }
    }
}
