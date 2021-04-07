using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class IngredientUnitTypeRelation : ICountIdentifiedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountId { get; set; }

        public string IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int UnitTypeId { get; set; }
        public UnitType UnitType { get; set; }

        public bool Deleted { get; set; }
    }
}
