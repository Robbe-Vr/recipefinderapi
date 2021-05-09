using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class Kitchen
    {
        [NotMapped]
        public string UserId { get; set; }
        [NotMapped]
        public User User { get; set; }

        [NotMapped]
        public ICollection<KitchenIngredient> Ingredients { get; set; }

        [NotMapped]
        public bool Deleted { get; set; }
    }

    public class KitchenIngredient : ICountIdentifiedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountId { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public double Units { get; set; }

        public int UnitTypeId { get; set; }
        public UnitType UnitType { get; set; }

        public bool Deleted { get; set; }
    }
}
