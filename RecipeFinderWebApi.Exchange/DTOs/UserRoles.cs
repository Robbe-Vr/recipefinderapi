using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class UserRoles
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Role> Roles { get; set; }

        public bool Deleted { get; set; }
    }
}
