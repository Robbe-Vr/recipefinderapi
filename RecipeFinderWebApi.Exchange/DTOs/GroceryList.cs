using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class GroceryList
    {
        public string Id { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }

        public bool Deleted { get; set; }
    }
}
