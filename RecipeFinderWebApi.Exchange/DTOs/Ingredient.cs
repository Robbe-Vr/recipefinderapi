using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class Ingredient : IDoubleIdentityfiedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountId { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }

        public string ImageLocation { get; set; }

        public double AverageWeightInKgPerUnit { get; set; }
        public double AverageVolumeInLiterPerUnit { get; set; }

        public ICollection<IngredientCategory> Categories { get; set; }
        public ICollection<UnitType> UnitTypes { get; set; }

        public bool Deleted { get; set; }
    }
}
