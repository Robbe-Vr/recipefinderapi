using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class Role : IDoubleIdentityfiedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public bool Deleted { get; set; }
    }
}
