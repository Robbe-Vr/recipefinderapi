using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using RecipeFinderWebApi.Logic.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.Logic
{
    public class AlgorithmHelper
    {
        private UnitTypeHandler _unitTypeHandler;
        private IIngredientRepo _ingredientRepo;

        public AlgorithmHelper(UnitTypeHandler unitTypeHandler, IIngredientRepo ingredientRepo)
        {
            _unitTypeHandler = unitTypeHandler;
            _ingredientRepo = ingredientRepo;
        }

        public UnitType LastUsed { get; private set; }

        public double[] EvenOutUnits(KitchenIngredient present, RequirementsListIngredient required)
        {
            double presentEqualAmount = 0.0;
            double requiredEqualAmount = 0.0;

            if (present.UnitTypeId == required.UnitTypeId)
            {
                presentEqualAmount = present.Units;
                requiredEqualAmount = required.Units;

                LastUsed = present.UnitType;
            }
            else
            {
                if (present.Ingredient.UnitTypes.Any(x => x.CountId == _unitTypeHandler.GetByName("Kg").CountId))
                {
                    if (present.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        presentEqualAmount = present.Units;
                    }
                    else if (present.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        presentEqualAmount = (present.Units / present.Ingredient.AverageVolumeInLiterPerUnit) * present.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (present.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        presentEqualAmount = present.Units * present.Ingredient.AverageWeightInKgPerUnit;
                    }

                    if (required.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        requiredEqualAmount = required.Units;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        requiredEqualAmount = (present.Units / present.Ingredient.AverageVolumeInLiterPerUnit) * present.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        requiredEqualAmount = required.Units * present.Ingredient.AverageWeightInKgPerUnit;
                    }

                    LastUsed = _unitTypeHandler.GetByName("Kg");
                }
                else if (present.Ingredient.UnitTypes.Any(x => x.CountId == _unitTypeHandler.GetByName("L").CountId))
                {
                    if (present.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        presentEqualAmount = present.Units;
                    }
                    else if (present.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        presentEqualAmount = (present.Units / present.Ingredient.AverageVolumeInLiterPerUnit) * present.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (present.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        presentEqualAmount = present.Units * present.Ingredient.AverageVolumeInLiterPerUnit;
                    }

                    if (required.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        requiredEqualAmount = (required.Units / required.Ingredient.AverageWeightInKgPerUnit) * required.Ingredient.AverageVolumeInLiterPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        requiredEqualAmount = required.Units;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        requiredEqualAmount = required.Units * required.Ingredient.AverageVolumeInLiterPerUnit;
                    }

                    LastUsed = _unitTypeHandler.GetByName("L");
                }
                else if (present.Ingredient.UnitTypes.Any(x => x.CountId == _unitTypeHandler.GetByName("Units").CountId))
                {
                    if (present.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        presentEqualAmount = present.Units / present.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (present.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        presentEqualAmount = present.Units / present.Ingredient.AverageVolumeInLiterPerUnit;
                    }
                    else if (present.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        presentEqualAmount = present.Units;
                    }

                    if (required.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        requiredEqualAmount = required.Units / required.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        requiredEqualAmount = required.Units / required.Ingredient.AverageVolumeInLiterPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
                    {
                        requiredEqualAmount = required.Units;
                    }

                    LastUsed = _unitTypeHandler.GetByName("Units");
                }
            }

            return new double[] { presentEqualAmount, requiredEqualAmount };
        }

        public double Convert(KitchenIngredient ingredient, UnitType toUnitType)
        {
            if (ingredient.Ingredient == null || ingredient.Ingredient.UnitTypes == null)
            {
                ingredient.Ingredient = _ingredientRepo.GetById(ingredient.IngredientId);
            }

            if (ingredient.UnitType == null)
            {
                ingredient.UnitType = _unitTypeHandler.GetById(ingredient.UnitTypeId);
            }

            if (ingredient.Ingredient.UnitTypes.Any(x => x.CountId == toUnitType.CountId))
            {
                if (ingredient.UnitTypeId == toUnitType.CountId)
                {
                    return ingredient.Units;
                }
                else if (ingredient.UnitType.Name == "Units")
                {
                    if (toUnitType.Name == "Kg")
                    {
                        return ingredient.Units * ingredient.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (toUnitType.Name == "L")
                    {
                        return ingredient.Units * ingredient.Ingredient.AverageVolumeInLiterPerUnit;
                    }
                }
                else if (ingredient.UnitType.Name == "Kg")
                {
                    if (toUnitType.Name == "Units")
                    {
                        return (int)((ingredient.Units / ingredient.Ingredient.AverageWeightInKgPerUnit) + 0.5);
                    }
                    else if (toUnitType.Name == "L")
                    {
                        return (ingredient.Units / ingredient.Ingredient.AverageWeightInKgPerUnit) * ingredient.Ingredient.AverageVolumeInLiterPerUnit;
                    }
                }
                else if (ingredient.UnitType.Name == "L")
                {
                    if (toUnitType.Name == "Kg")
                    {
                        return (ingredient.Units / ingredient.Ingredient.AverageVolumeInLiterPerUnit) * ingredient.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (toUnitType.Name == "Units")
                    {
                        return (int)((ingredient.Units / ingredient.Ingredient.AverageVolumeInLiterPerUnit) + 0.5);
                    }
                }
            }

            return 0;
        }
    }
}
