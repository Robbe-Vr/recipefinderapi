using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.DAL;
using RecipeFinderWebApi.DAL.Repositories;
using RecipeFinderWebApi.Exchange.DTOs;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace RecipeFinderWebApi.UnitTests
{
    [TestClass]
    public class WhatToBuyAlgorithmTests
    {
        private IngredientHandler handler;
        private KitchenHandler kitchenHandler;

        private User user;

        private RecipeWithRequirements recipe1;
        private RecipeWithRequirements recipe2;

        private RecipeCategory category1;
        private RecipeCategory category2;

        private Ingredient ingredient1;
        private Ingredient ingredient2;

        private UnitType unitType1;
        private UnitType unitType2;
        private UnitType unitType3;

        public WhatToBuyAlgorithmTests()
        {
        }

        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB-whattobuy", new InMemoryDatabaseRoot());
            builder.EnableSensitiveDataLogging();
            builder.ConfigureWarnings(options =>
            {
                options.Ignore(new[] { CoreEventId.ManyServiceProvidersCreatedWarning });
            });

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            user = new User()
            {
                CountId = 1,
                Id = "8fs9d8fds-A908fsd9sfd80a-f7s8dr23",
                Name = "Recipe Finder admin",
                Email = "admin@recipefinder.com",
            };

            context.Users.Add(user);
            context.SaveChanges();
            user = context.Users.First(x => x.Name == user.Name);

            kitchenHandler = new KitchenHandler(new KitchenRepo(context), new IngredientRepo(context), new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context)));
            RecipeHandler recipeHandler = new RecipeHandler(new RecipeRepo(context, user), new RecipeCategoryRelationRepo(context), new RequirementsListRepo(context),
                new Logic.PreparableRecipesAlgorithm(null, null, null, null));
            handler = new IngredientHandler(new IngredientRepo(context), new IngredientCategoryRelationRepo(context), new IngredientUnitTypeRelationRepo(context),
                new Logic.WhatToBuyAlgorithm(recipeHandler, kitchenHandler, new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context)), new IngredientRepo(context)));

            category1 = new RecipeCategory() { Name = "TestCategory1" };
            category2 = new RecipeCategory() { Name = "TestCategory2" };
            context.RecipeCategories.AddRange(new[] { category1, category2 });
            context.SaveChanges();
            category1 = context.RecipeCategories.First(x => x.Name == category1.Name);
            category2 = context.RecipeCategories.First(x => x.Name == category2.Name);

            unitType1 = new UnitType() { Name = "Units", AllowDecimals = true };
            unitType2 = new UnitType() { Name = "Liter", AllowDecimals = true };
            unitType3 = new UnitType() { Name = "Kg", AllowDecimals = true };
            context.UnitTypes.AddRange(new[] { unitType1, unitType2, unitType3 });
            context.SaveChanges();
            unitType1 = context.UnitTypes.First(x => x.Name == unitType1.Name);
            unitType2 = context.UnitTypes.First(x => x.Name == unitType2.Name);
            unitType3 = context.UnitTypes.First(x => x.Name == unitType3.Name);

            ingredient1 = new Ingredient()
            {
                Id = "75483",
                Name = "Ingredient1",
                AverageVolumeInLiterPerUnit = 0.2,
                AverageWeightInKgPerUnit = 0.0,
                Categories = new List<IngredientCategory>(),
                ImageLocation = "",
                UnitTypes = new List<UnitType>()
                    { unitType1, unitType2 }
            };
            ingredient2 = new Ingredient()
            {
                Id = "75484",
                Name = "Ingredient2",
                AverageVolumeInLiterPerUnit = 0.0,
                AverageWeightInKgPerUnit = 0.6,
                Categories = new List<IngredientCategory>(),
                ImageLocation = "",
                UnitTypes = new List<UnitType>()
                    { unitType1, unitType3 }
            };
            context.Ingredients.AddRange(new[] { ingredient1, ingredient2 });
            context.SaveChanges();
            ingredient1 = context.Ingredients.First(x => x.Name == ingredient1.Name);
            ingredient2 = context.Ingredients.First(x => x.Name == ingredient2.Name);

            recipe1 = new RecipeWithRequirements()
            {
                Id = "75483",
                Name = "Test1",
                Categories = new List<RecipeCategory>() { category1 },
                PreparationSteps = "",
                Description = "",
                VideoTutorialLink = "",
                User = user,
                RequirementsList = new RequirementsList()
                {
                    Ingredients = new List<RequirementsListIngredient>()
                },
                Deleted = false,
            };
            recipe2 = new RecipeWithRequirements()
            {
                Id = "75484",
                Name = "Test2",
                Categories = new List<RecipeCategory>() { category2 },
                PreparationSteps = "",
                Description = "",
                VideoTutorialLink = "",
                User = user,
                RequirementsList = new RequirementsList()
                {
                    Ingredients = new List<RequirementsListIngredient>()
                },
                Deleted = false,
            };
            context.Recipes.AddRange(new[] { recipe1, recipe2 });
            context.SaveChanges();
            recipe1 = new RecipeWithRequirements(context.Recipes.First(x => x.Name == recipe1.Name));
            recipe2 = new RecipeWithRequirements(context.Recipes.First(x => x.Name == recipe2.Name));

            context.RequirementsLists.AddRange(new List<RequirementsListIngredient>()
                {
                    new RequirementsListIngredient() { Ingredient = ingredient1, IngredientId = ingredient1.Id, Units = 5, UnitType = unitType1, UnitTypeId = unitType1.CountId, RecipeId = recipe1.Id },
                    new RequirementsListIngredient() { Ingredient = ingredient2, IngredientId = ingredient2.Id, Units = 4, UnitType = unitType1, UnitTypeId = unitType1.CountId, RecipeId = recipe1.Id },
                });
            context.RequirementsLists.AddRange(new List<RequirementsListIngredient>()
                {
                    new RequirementsListIngredient() { Ingredient = ingredient1, IngredientId = ingredient1.Id, Units = 8, UnitType = unitType1, UnitTypeId = unitType1.CountId, RecipeId = recipe2.Id },
                });
            context.SaveChanges();
        }

        [TestMethod]
        public void TestAlgorithmWithCompleteKitchen()
        {
            Initialize();

            kitchenHandler.Create(new KitchenIngredient()
            {
                Ingredient = ingredient1,
                IngredientId = ingredient1.Id,
                UnitType = unitType1,
                UnitTypeId = unitType1.CountId,
                Units = 10, // enough for first and second recipe
                User = user,
                UserId = user.Id,
                CountId = 1,
                Deleted = false,
            });
            kitchenHandler.Create(new KitchenIngredient()
            {
                Ingredient = ingredient2,
                IngredientId = ingredient2.Id,
                UnitType = unitType1,
                UnitTypeId = unitType1.CountId,
                Units = 10, // enough for second recipe's second ingredient
                User = user,
                UserId = user.Id,
                CountId = 2,
                Deleted = false,
            });

            var recipes = handler.GetWhatToBuyInRecipesForUser(user.Id);

            Assert.AreEqual(0, recipes.Count());
        }

        [TestMethod]
        public void TestAlgorithmWithPartialKitchen()
        {
            Initialize();

            kitchenHandler.Create(new KitchenIngredient()
            {
                Ingredient = ingredient1,
                IngredientId = ingredient1.Id,
                UnitType = unitType1,
                UnitTypeId = unitType1.CountId,
                Units = 10, // enough for first and second recipe, missing ingredient for second recipe
                User = user,
                UserId = user.Id,
                CountId = 1,
                Deleted = false,
            });

            var recipes = handler.GetWhatToBuyInRecipesForUser(user.Id);

            Assert.AreEqual(0, recipes.Count());
            //Assert.AreEqual(1, recipes.Count());
            //Assert.AreEqual(4, recipes.ToArray()[0].RequirementsList.Ingredients.ToArray()[0].Units);
        }

        [TestMethod]
        public void TestAlgorithmWithIncompleteKitchen()
        {
            Initialize();

            kitchenHandler.Create(new KitchenIngredient()
            {
                Ingredient = ingredient1,
                IngredientId = ingredient1.Id,
                UnitType = unitType1,
                UnitTypeId = unitType1.CountId,
                Units = 2, // not enough for first and second recipe, missing ingredient for second recipe
                User = user,
                UserId = user.Id,
                CountId = 1,
                Deleted = false,
            });

            var recipes = handler.GetWhatToBuyInRecipesForUser(user.Id);

            Assert.AreEqual(2, recipes.Count());
            Assert.AreEqual(6, recipes.First(x => x.Id == recipe2.Id).RequirementsList.Ingredients.First(x => x.IngredientId == ingredient1.Id).Units);
            Assert.AreEqual(3, recipes.First(x => x.Id == recipe1.Id).RequirementsList.Ingredients.First(x => x.IngredientId == ingredient1.Id).Units);
            //Assert.AreEqual(4, recipes.First(x => x.Id == recipe1.Id).RequirementsList.Ingredients.First(x => x.IngredientId == ingredient2.Id).Units);
        }

        [TestMethod]
        public void TestAlgorithmWithEmptyKitchen()
        {
            Initialize();

            var recipes = handler.GetWhatToBuyInRecipesForUser(user.Id);

            Assert.AreEqual(0, recipes.Count());
        }
    }
}
