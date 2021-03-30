using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class RecipeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Recipe> Recipes { get; set; }

        public bool Deleted { get; set; }
    }
}
