using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
{
    public class GroceryListHandler
    {
        private IGroceryListRepo _repo;

        public GroceryListHandler(IGroceryListRepo repo)
        {
            _repo = repo;
        }

        public IEnumerable<GroceryList> GetAll()
        {
            return _repo.GetAll();
        }

        public IEnumerable<GroceryList> GetAllByUserId(string id)
        {
            return _repo.GetAllByUserId(id);
        }

        public GroceryList GetById(string id)
        {
            return _repo.GetById(id);
        }

        public GroceryList GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(GroceryList list)
        {
            if (!Validate(list))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(list);

            if (validationResult != 0)
            {
                if (validationResult == -2)
                {
                    if (!_repo.TryRestore(list))
                    {
                        return 0;
                    }
                }
                
                return validationResult;
            }

            return _repo.Create(list);
        }

        public int Update(GroceryList list)
        {
            if (!Validate(list))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(list);

            if (validationResult != 0)
            {
                if (validationResult != -2)
                {
                    return validationResult;
                }
                else if (_repo.TryRestore(list))
                {
                    return validationResult;
                }
            }

            return _repo.Update(list);
        }

        public int Delete(GroceryList list)
        {
            return _repo.Delete(GetById(list.Id));
        }

        private bool Validate(GroceryList list)
        {
            return (list.Name?.Length > 2 && list.UserId?.Length > 1 && list.Value?.Split(',').Length >= 3);
        }
    }
}
