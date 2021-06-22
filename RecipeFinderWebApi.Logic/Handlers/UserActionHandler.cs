using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
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
            int validationResult = _repo.ValidateOriginality(action);

            if (validationResult != 0)
            {
                if (validationResult == -2)
                {
                    if (!_repo.TryRestore(action))
                    {
                        return 0;
                    }
                }

                return validationResult;
            }

            return _repo.Create(action);
        }

        public int Update(UserAction action)
        {
            int validationResult = _repo.ValidateOriginality(action);

            if (validationResult != 0)
            {
                if (validationResult != -2)
                {
                    return validationResult;
                }
                else if (_repo.TryRestore(action))
                {
                    return validationResult;
                }
            }

            return _repo.Update(action);
        }

        public int Delete(UserAction action)
        {
            return _repo.Delete(GetById(action.CountId));
        }
    }
}
