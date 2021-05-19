using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class Recipe : IDoubleIdentityfiedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<RecipeCategory> Categories { get; set; }

        public bool IsPublic { get; set; } = true;
        public string VideoTutorialLink { get; set; }
        public string ImageLocation { get; set; }
        public string Description { get; set; }
        public string PreparationSteps { get; set; }

        public bool Deleted { get; set; }
    }
}
