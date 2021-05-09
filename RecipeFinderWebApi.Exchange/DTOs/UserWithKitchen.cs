using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class UserWithKitchen : User
    {
        public Kitchen Kitchen { get; set; }
    }
}
