using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
{
    public class UnitTypeHandler
    {
        private IUnitTypeRepo _repo;

        private IIngredientUnitTypeRelationRepo _ingredient_relation_repo;
        public UnitTypeHandler(IUnitTypeRepo repo, IIngredientUnitTypeRelationRepo ingredient_relation_repo)
        {
            _repo = repo;

            _ingredient_relation_repo = ingredient_relation_repo;
        }

        public IEnumerable<UnitType> GetAll()
        {
            return _repo.GetAll();
        }

        public UnitType GetById(int id)
        {
            return _repo.GetById(id);
        }

        public UnitType GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(UnitType unitType)
        {
            if (!Validate(unitType))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(unitType);

            if (validationResult != 0)
            {
                if (validationResult == -2)
                {
                    if (!_repo.TryRestore(unitType))
                    {
                        return 0;
                    }
                }

                return validationResult;
            }

            return _repo.Create(unitType);
        }

        public int CreateIngredientRelation(Ingredient ingredient, UnitType unitType)
        {
            return _ingredient_relation_repo.CreateRelation(ingredient, unitType);
        }

        public int DeleteIngredientRelation(IngredientUnitTypeRelation relation)
        {
            return _ingredient_relation_repo.DeleteRelation(relation);
        }

        public int DeleteIngredientRelation(Ingredient ingredient, UnitType category)
        {
            var relation = _ingredient_relation_repo.GetByIngredientIdAndUnitTypeId(ingredient.Id, category.CountId);

            return _ingredient_relation_repo.DeleteRelation(relation);
        }

        public int Update(UnitType unitType)
        {
            if (!Validate(unitType))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(unitType);

            if (validationResult != 0)
            {
                if (validationResult != -2)
                {
                    return validationResult;
                }
                else if (_repo.TryRestore(unitType))
                {
                    return validationResult;
                }
            }

            return _repo.Update(unitType);
        }

        public int Delete(UnitType unitType)
        {
            return _repo.Delete(GetById(unitType.CountId));
        }

        private bool Validate(UnitType unitType)
        {
            return (unitType.Name?.Length > 0);
        }
    }
}
