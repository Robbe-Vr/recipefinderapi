﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class IngredientCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public bool Deleted { get; set; }
    }
}
