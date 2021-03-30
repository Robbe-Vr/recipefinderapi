using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public bool Deleted { get; set; }
    }
}
