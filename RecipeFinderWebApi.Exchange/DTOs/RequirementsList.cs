using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class RequirementsList
    {
        [NotMapped]
        public string RecipeId { get; set; }
        [NotMapped]
        public Recipe Recipe { get; set; }

        [NotMapped]
        public ICollection<RequirementsListIngredient> Ingredients { get; set; }

        [NotMapped]
        public bool Deleted { get; set; }
    }

    public class RequirementsListIngredient : ICountIdentifiedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountId { get; set; }

        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public string IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public double Units { get; set; }

        public int UnitTypeId { get; set; }
        public UnitType UnitType { get; set; }

        public bool Deleted { get; set; }
    }
}
