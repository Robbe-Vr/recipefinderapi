using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWabApi.Logic.Handlers
{
    public class IngredientHandler
    {
        private IIngredientRepo _repo;

        private IIngredientCategoryRelationRepo _category_relation_repo;
        private IIngredientUnitTypeRelationRepo _unitType_relation_repo;

        public IngredientHandler(IIngredientRepo repo, IIngredientCategoryRelationRepo category_relation_repo, IIngredientUnitTypeRelationRepo unitType_relation_repo)
        {
            _repo = repo;

            _category_relation_repo = category_relation_repo;
            _unitType_relation_repo = unitType_relation_repo;
        }

        public IEnumerable<Ingredient> GetAll()
        {
            return _repo.GetAll();
        }

        public Ingredient GetById(string id)
        {
            return _repo.GetById(id);
        }

        public Ingredient GetByName(string name)
        {
            return _repo.GetByName(name);
        }

        public int Create(Ingredient ingredient)
        {
            int changes = 0;

            List<IngredientCategory> Categories = new List<IngredientCategory>();
            Categories.AddRange(ingredient.Categories);

            List<UnitType> UnitTypes = new List<UnitType>();
            UnitTypes.AddRange(ingredient.UnitTypes);

            changes += _repo.Create(ingredient);

            if (Categories.Count > 0)
            {
                foreach (IngredientCategory category in Categories)
                {
                    changes += CreateCategoryRelation(ingredient, category);
                }
            }

            if (UnitTypes.Count > 0)
            {
                foreach (UnitType unitType in UnitTypes)
                {
                    changes += CreateUnitTypeRelation(ingredient, unitType);
                }
            }

            return changes;
        }

        public Ingredient CreateGetId(Ingredient ingredient)
        {
            List<IngredientCategory> Categories = new List<IngredientCategory>();
            Categories.AddRange(ingredient.Categories);

            List<UnitType> UnitTypes = new List<UnitType>();
            UnitTypes.AddRange(ingredient.UnitTypes);

            ingredient = _repo.CreateGetId(ingredient);

            if (Categories.Count > 0)
            {
                foreach (IngredientCategory category in Categories)
                {
                    CreateCategoryRelation(ingredient, category);
                }
            }

            if (UnitTypes.Count > 0)
            {
                foreach (UnitType unitType in UnitTypes)
                {
                    CreateUnitTypeRelation(ingredient, unitType);
                }
            }

            return ingredient;
        }

        public int Update(Ingredient ingredient)
        {
            int changes = 0;

            var currentState = GetById(ingredient.Id);

            ingredient.CountId = currentState.CountId;

            List<IngredientCategory> Categories = new List<IngredientCategory>();
            Categories.AddRange(ingredient.Categories);

            List<UnitType> UnitTypes = new List<UnitType>();
            UnitTypes.AddRange(ingredient.UnitTypes);

            changes += _repo.Update(ingredient);

            if (Categories.Count > 0)
            {
                IEnumerable<IngredientCategory> toAddCategories = Categories.Where(x => !currentState.Categories.Select(x => x.CountId).Contains(x.CountId));

                foreach (IngredientCategory category in toAddCategories)
                {
                    changes += CreateCategoryRelation(ingredient, category);
                }

                IEnumerable<IngredientCategory> toRemoveCategories = currentState.Categories.Where(x => !Categories.Select(x => x.CountId).Contains(x.CountId));

                foreach (IngredientCategory category in toRemoveCategories)
                {
                    changes += DeleteCategoryRelation(ingredient, category);
                }
            }

            if (UnitTypes.Count > 0)
            {
                IEnumerable<UnitType> toAddUnitTypes = UnitTypes.Where(x => !currentState.UnitTypes.Select(x => x.CountId).Contains(x.CountId));

                foreach (UnitType unitType in toAddUnitTypes)
                {
                    changes += CreateUnitTypeRelation(ingredient, unitType);
                }

                IEnumerable<UnitType> toRemoveUnitTypes = currentState.UnitTypes.Where(x => !UnitTypes.Select(x => x.CountId).Contains(x.CountId));

                foreach (UnitType unitType in toRemoveUnitTypes)
                {
                    changes += DeleteUnitTypeRelation(ingredient, unitType);
                }
            }

            return changes;
        }

        public int Delete(Ingredient ingredient)
        {
            int changes = 0;

            var currentState = GetById(ingredient.Id);

            List<IngredientCategory> Categories = new List<IngredientCategory>();
            Categories.AddRange(ingredient.Categories);

            List<UnitType> UnitTypes = new List<UnitType>();
            UnitTypes.AddRange(ingredient.UnitTypes);

            changes += _repo.Delete(currentState);

            foreach (IngredientCategory category in Categories)
            {
                changes += DeleteCategoryRelation(ingredient, category);
            }

            foreach (UnitType unitType in UnitTypes)
            {
                changes += DeleteUnitTypeRelation(ingredient, unitType);
            }

            return changes;
        }

        public int CreateCategoryRelation(Ingredient ingredient, IngredientCategory category)
        {
            return _category_relation_repo.CreateRelation(ingredient, category);
        }

        public int DeleteCategoryRelation(IngredientCategoryRelation relation)
        {
            return _category_relation_repo.DeleteRelation(relation);
        }

        public int DeleteCategoryRelation(Ingredient ingredient, IngredientCategory category)
        {
            var relation = _category_relation_repo.GetByIngredientIdAndCategoryId(ingredient.Id, category.CountId);

            return _category_relation_repo.DeleteRelation(relation);
        }

        public int CreateUnitTypeRelation(Ingredient ingredient, UnitType category)
        {
            return _unitType_relation_repo.CreateRelation(ingredient, category);
        }

        public int DeleteUnitTypeRelation(IngredientUnitTypeRelation relation)
        {
            return _unitType_relation_repo.DeleteRelation(relation);
        }

        public int DeleteUnitTypeRelation(Ingredient ingredient, UnitType category)
        {
            var relation = _unitType_relation_repo.GetByIngredientIdAndUnitTypeId(ingredient.Id, category.CountId);

            return _unitType_relation_repo.DeleteRelation(relation);
        }

    }
}
