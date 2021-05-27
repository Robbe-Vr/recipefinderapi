using RecipeFinderWebApi.Exchange.DTOs;
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

        public AlgorithmHelper(UnitTypeHandler unitTypeHandler)
        {
            _unitTypeHandler = unitTypeHandler;
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
            }
            else
            {
                if (present.Ingredient.UnitTypes.Any(x => x.CountId == _unitTypeHandler.GetByName("Units").CountId))
                {
                    if (present.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        presentEqualAmount = present.Units / present.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (present.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        presentEqualAmount = present.Units / present.Ingredient.AverageVolumeInLiterPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
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
                else if (present.Ingredient.UnitTypes.Any(x => x.CountId == _unitTypeHandler.GetByName("Kg").CountId))
                {
                    if (present.UnitTypeId == _unitTypeHandler.GetByName("Kg").CountId)
                    {
                        presentEqualAmount = present.Units;
                    }
                    else if (present.UnitTypeId == _unitTypeHandler.GetByName("L").CountId)
                    {
                        presentEqualAmount = (present.Units / present.Ingredient.AverageVolumeInLiterPerUnit) * present.Ingredient.AverageWeightInKgPerUnit;
                    }
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
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
                    else if (required.UnitTypeId == _unitTypeHandler.GetByName("Units").CountId)
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
            }

            return new double[] { presentEqualAmount, requiredEqualAmount };
        }
    }
}
