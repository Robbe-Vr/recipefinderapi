using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
{
    public class UserHandler
    {
        private IUserRepo _repo;

        public UserHandler(IUserRepo repo)
        {
            _repo = repo;
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

        public int Create(User user)
        {
            return _repo.Create(user);
        }

        public int Update(User user)
        {
            return _repo.Update(user);
        }

        public int Delete(User user)
        {
            return _repo.Delete(user);
        }
    }
}
