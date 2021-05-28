using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.DAL;
using RecipeFinderWebApi.DAL.Repositories;
using RecipeFinderWebApi.Exchange.DTOs;
using System.Collections.Generic;
using System.Linq;

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

        [TestInitialize()]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB");
            builder.EnableSensitiveDataLogging();

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            User user = new User()
            {
                CountId = 1,
                Id = "8fs9d8fds-A908fsd9sfd80a-f7s8dr23",
                Name = "Recipe Finder admin",
                Email = "admin@recipefinder.com",
            };

            handler = new RecipeHandler(new RecipeRepo(context, user), new RecipeCategoryRelationRepo(context), new RequirementsListRepo(context),
                new Logic.PreparableRecipesAlgorithm(null, null, null));
            categoryHandler = new RecipeCategoryHandler(new RecipeCategoryRepo(context), new RecipeCategoryRelationRepo(context));
            ingredientHandler = new IngredientHandler(new IngredientRepo(context), new IngredientCategoryRelationRepo(context), new IngredientUnitTypeRelationRepo(context),
                null);
            unitTypeHandler = new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context));

            var recipes = handler.GetAll();

            if (recipes.Any())
            {
                foreach (var recipe in recipes)
                {
                    handler.Delete(recipe);
                }
            }

            category1 = new RecipeCategory() { Name = "TestCategory1" };
            category2 = new RecipeCategory() { Name = "TestCategory2" };
            categoryHandler.Create(category1);
            categoryHandler.Create(category2);

            unitType1 = new UnitType() { Name = "Units", AllowDecimals = true };
            unitType2 = new UnitType() { Name = "Liter", AllowDecimals = true };
            unitTypeHandler.Create(unitType1);
            unitTypeHandler.Create(unitType2);

            ingredient1 = new Ingredient()
                { Name = "Ingredient1", AverageVolumeInLiterPerUnit = 0.2, AverageWeightInKgPerUnit = 0.0, Categories = new List<IngredientCategory>(), ImageLocation = "", UnitTypes = new List<UnitType>()
                    { unitType1, unitType2 } };
            ingredientHandler.Create(ingredient1);
            ingredient1 = ingredientHandler.GetByName(ingredient1.Name);

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
            var recipes = handler.GetAll();

            Assert.AreEqual(recipes.Count(), 0);
        }

        [TestMethod]
        public void TestCreate()
        {
            Create();
        }

        [TestMethod]
        public void TestGetByName()
        {
            Create();

            GetByName();
        }

        [TestMethod]
        public void TestGetById()
        {
            Create();

            GetByName();
            GetById();
        }

        [TestMethod]
        public void TestUpdate()
        {
            Create();

            GetByName();
            GetById();

            Update();
        }

        [TestMethod]
        public void TestDelete()
        {
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
        }

        public void GetByName()
        {
            Recipe bynameRecipe = handler.GetByName(recipe.Name);

            recipe.Id = bynameRecipe.Id;

            Assert.AreEqual(recipe.Name, bynameRecipe.Name);
        }

        public void GetById()
        {
            RecipeWithRequirements byidRecipe = handler.GetById(recipe.Id);

            recipe = byidRecipe;

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

            Assert.AreEqual(3, updatedRows);
            Assert.AreEqual(updatedName, handler.GetById(recipe.Id).Name);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(recipe);

            Assert.AreEqual(3, removedRows);

            Assert.IsNull(handler.GetById(recipe.Id));
        }
    }
}
