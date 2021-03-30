using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string PasswordHashed { get; set; }
        public string Salt { get; set; }

        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        public string NAME_NORMALIZED { get; set; }
        public string EMAIL_NORMALIZED { get; set; }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string EmailConfirmationToken { get; set; }

        public string SecurityStamp { get; set; }

        public string ConcurrencyStamp { get; set; }

        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }

        public ICollection<Role> Roles { get; set; }

        public Kitchen Kitchen { get; set; }

        public bool Deleted { get; set; }
    }
}
