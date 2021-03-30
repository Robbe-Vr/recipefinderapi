using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class UserRoleRelation
    {
        public int CountId { get; set; }
        
        public string UserId { get; set; }
        public User User { get; set; }

        public string RoleId { get; set; }
        public Role Role { get; set; }

        public bool Deleted { get; set; }
    }
}
