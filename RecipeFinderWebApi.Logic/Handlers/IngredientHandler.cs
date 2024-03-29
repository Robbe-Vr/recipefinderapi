﻿using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeFinderWebApi.Logic.Handlers
{
    public class IngredientHandler
    {
        private IIngredientRepo _repo;

        private IIngredientCategoryRelationRepo _category_relation_repo;
        private IIngredientUnitTypeRelationRepo _unitType_relation_repo;

        private WhatToBuyAlgorithm _algorithm;

        public IngredientHandler(IIngredientRepo repo, IIngredientCategoryRelationRepo category_relation_repo, IIngredientUnitTypeRelationRepo unitType_relation_repo, WhatToBuyAlgorithm algorithm)
        {
            _repo = repo;

            _category_relation_repo = category_relation_repo;
            _unitType_relation_repo = unitType_relation_repo;

            _algorithm = algorithm;
        }

        public IEnumerable<Ingredient> GetAll()
        {
            return _repo.GetAll();
        }

        public IEnumerable<RecipeWithRequirements> GetWhatToBuyInRecipesForUser(string userId)
        {
            return _algorithm.GetWhatToBuyInRecipesForUser(userId);
        }

        public IEnumerable<RequirementsListIngredient> GetWhatToBuyInIngredientsForUser(string userId)
        {
            return _algorithm.GetWhatToBuyInIngredientsForUser(userId);
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
            if (!Validate(ingredient))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(ingredient);

            if (validationResult != 0)
            {
                if (validationResult == -2)
                {
                    if (!_repo.TryRestore(ingredient))
                    {
                        return 0;
                    }
                }

                return validationResult;
            }

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
            if (!Validate(ingredient))
            {
                return null;
            }

            int validationResult = _repo.ValidateOriginality(ingredient);

            if (validationResult != 0)
            {
                if (validationResult == -2)
                {
                    if (!_repo.TryRestore(ingredient))
                    {
                        return null;
                    }
                }

                return null;
            }

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
            if (!Validate(ingredient))
            {
                return -10;
            }

            int validationResult = _repo.ValidateOriginality(ingredient);

            if (validationResult != 0)
            {
                if (validationResult != -2)
                {
                    return validationResult;
                }
                else if (_repo.TryRestore(ingredient))
                {
                    return validationResult;
                }
            }

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
            Categories.AddRange(currentState.Categories);

            List<UnitType> UnitTypes = new List<UnitType>();
            UnitTypes.AddRange(currentState.UnitTypes);

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

        private bool Validate(Ingredient ingredient)
        {
            return (ingredient.Name?.Length > 2 && 
                (ingredient.UnitTypes?.Any(x => x.CountId == 10) == true ? ingredient.AverageVolumeInLiterPerUnit > 0.00 : true) &&
                (ingredient.UnitTypes?.Any(x => x.CountId == 11) == true ? ingredient.AverageWeightInKgPerUnit > 0.00 : true) &&
                ingredient.Categories?.Count > 0);
        }
    }
}
