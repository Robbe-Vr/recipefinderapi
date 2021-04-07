using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class GroceryList : IDoubleIdentityfiedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountId { get; set; }

        public string Id { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }

        public bool Deleted { get; set; }
    }
}
