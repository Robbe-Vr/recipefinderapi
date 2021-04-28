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

        public int Create(UnitType ingredient)
        {
            return _repo.Create(ingredient);
        }

        public int CreateIngredientRelation(Ingredient ingredient, UnitType category)
        {
            return _ingredient_relation_repo.CreateRelation(ingredient, category);
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

        public int Update(UnitType ingredient)
        {
            return _repo.Update(ingredient);
        }

        public int Delete(UnitType ingredient)
        {
            return _repo.Delete(ingredient);
        }
    }
}
