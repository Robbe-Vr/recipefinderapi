using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class Recipe
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<RecipeCategory> Categories { get; set; }

        public string VideoTutorialLink { get; set; }
        public string Description { get; set; }
        public string PreparationSteps { get; set; }

        public RequirementsList RequirementsList { get; set; }

        public bool Deleted { get; set; }
    }
}
