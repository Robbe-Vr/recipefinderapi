using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class RecipeWithRequirements : Recipe
    {
        public RecipeWithRequirements() { }

        public RecipeWithRequirements(Recipe recipe)
        {
            if (recipe == null) return;

            CountId = recipe.CountId;
            Id = recipe.Id;
            Name = recipe.Name;
            ImageLocation = recipe.ImageLocation;
            IsPublic = recipe.IsPublic;
            Description = recipe.Description;
            VideoTutorialLink = recipe.VideoTutorialLink;
            ImageLocation = recipe.ImageLocation;
            PreparationSteps = recipe.PreparationSteps;
            Categories = recipe.Categories;
            User = recipe.User;
            UserId = recipe.UserId;
            Deleted = recipe.Deleted;
        }

        public RequirementsList RequirementsList { get; set; }
    }
}
