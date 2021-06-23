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
    public class RecipeCRUDTests
    {
        private RecipeHandler handler;
        private RecipeCategoryHandler categoryHandler;
        private IngredientHandler ingredientHandler;
        private UnitTypeHandler unitTypeHandler;

        private RecipeWithRequirements recipe;

        private RecipeCategory category1;
        private RecipeCategory category2;

        private Ingredient ingredient1;

        private UnitType unitType1;
        private UnitType unitType2;

        public RecipeCRUDTests()
        {
        }

        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB-recipes", new InMemoryDatabaseRoot());
            builder.EnableSensitiveDataLogging();
            builder.ConfigureWarnings(options =>
            {
                options.Ignore(new[] { CoreEventId.ManyServiceProvidersCreatedWarning });
            });

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            User user = new User()
            {
                CountId = 1,
                Id = "8fs9d8fds-A908fsd9sfd80a-f7s8dr23",
                Name = "Recipe Finder admin",
                Email = "admin@recipefinder.com",
            };

            handler = new RecipeHandler(new RecipeRepo(context, user), new RecipeCategoryRelationRepo(context), new RequirementsListRepo(context),
                new Logic.PreparableRecipesAlgorithm(null, null, null, null));
            categoryHandler = new RecipeCategoryHandler(new RecipeCategoryRepo(context), new RecipeCategoryRelationRepo(context));
            ingredientHandler = new IngredientHandler(new IngredientRepo(context), new IngredientCategoryRelationRepo(context), new IngredientUnitTypeRelationRepo(context),
                null);
            unitTypeHandler = new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context));

            category1 = new RecipeCategory() { Name = "TestCategory1" };
            category2 = new RecipeCategory() { Name = "TestCategory2" };
            context.RecipeCategories.AddRange(new[] { category1, category2 });
            context.SaveChanges();
            category1 = context.RecipeCategories.First(x => x.Name == category1.Name);
            category2 = context.RecipeCategories.First(x => x.Name == category2.Name);

            unitType1 = new UnitType() { Name = "Units", AllowDecimals = true };
            unitType2 = new UnitType() { Name = "Liter", AllowDecimals = true };
            context.UnitTypes.AddRange(new[] { unitType1, unitType2 });
            context.SaveChanges();
            unitType1 = context.UnitTypes.First(x => x.Name == unitType1.Name);
            unitType2 = context.UnitTypes.First(x => x.Name == unitType2.Name);

            ingredient1 = new Ingredient()
                { Id = "75483", Name = "Ingredient1", AverageVolumeInLiterPerUnit = 0.2, AverageWeightInKgPerUnit = 0.0, Categories = new List<IngredientCategory>(), ImageLocation = "", UnitTypes = new List<UnitType>()
                    { unitType1, unitType2 } };
            context.Ingredients.AddRange(new[] { ingredient1 });
            context.SaveChanges();
            ingredient1 = context.Ingredients.First(x => x.Name == ingredient1.Name);

            recipe = new RecipeWithRequirements()
            {
                Name = "Test",
                Categories = new List<RecipeCategory>() { category1 },
                PreparationSteps = "",
                Description = "",
                VideoTutorialLink = "",
                User = user,
                RequirementsList = new RequirementsList()
                    { Ingredients = new List<RequirementsListIngredient>()
                        { new RequirementsListIngredient() { Ingredient = ingredient1, IngredientId = ingredient1.Id, Units = 5, UnitType = unitType1, UnitTypeId = unitType1.CountId } } },
                Deleted = false,
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            Initialize();

            var recipes = handler.GetAll();

            Assert.AreEqual(0, recipes.Count());
        }

        [TestMethod]
        public void TestCreate()
        {
            Initialize();

            Create();
        }

        [TestMethod]
        public void TestGetByName()
        {
            Initialize();

            Create();

            GetByName();
        }

        [TestMethod]
        public void TestGetById()
        {
            Initialize();

            Create();

            GetByName();
            GetById();
        }

        [TestMethod]
        public void TestUpdate()
        {
            Initialize();

            Create();

            GetByName();
            GetById();

            Update();
        }

        [TestMethod]
        public void TestDelete()
        {
            Initialize();

            Create();

            GetByName();
            GetById();

            Delete();
        }

        public void Create()
        {
            int expectedNewRows = 3;

            int newRows = handler.Create(recipe);

            Assert.AreEqual(expectedNewRows, newRows);

            recipe = handler.GetByName(recipe.Name);
        }

        public void GetByName()
        {
            Recipe bynameRecipe = handler.GetByName(recipe.Name);

            Assert.AreEqual(recipe.Name, bynameRecipe.Name);
        }

        public void GetById()
        {
            RecipeWithRequirements byidRecipe = handler.GetById(recipe.Id);

            Assert.AreEqual(recipe.Name, byidRecipe.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            recipe.Name = updatedName;

            recipe.Categories.Add(category2);
            recipe.RequirementsList = new RequirementsList()
            {
                Ingredients = new List<RequirementsListIngredient>()
                    { new RequirementsListIngredient() { Ingredient = ingredient1, IngredientId = ingredient1.Id, Units = 7, UnitType = unitType1, UnitTypeId = unitType1.CountId } }
            };

            int updatedRows = handler.Update(recipe);

            Assert.IsTrue(updatedRows > 1);
            Assert.AreEqual(updatedName, handler.GetById(recipe.Id).Name);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(recipe);

            Assert.IsTrue(removedRows > 1);

            Assert.IsNull(handler.GetById(recipe.Id));
        }
    }
}
