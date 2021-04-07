using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class UnitType : ICountIdentifiedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountId { get; set; }
        public string Name { get; set; }
        public bool AllowDecimals { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public bool Deleted { get; set; }
    }
}
