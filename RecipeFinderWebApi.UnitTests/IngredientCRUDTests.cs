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
    public class IngredientCRUDTests
    {
        private IngredientHandler handler;
        private IngredientCategoryHandler categoryHandler;
        private UnitTypeHandler unitTypeHandler;

        private Ingredient ingredient;

        private IngredientCategory category1;
        private IngredientCategory category2;

        private UnitType unitType1;
        private UnitType unitType2;

        public IngredientCRUDTests()
        {
        }

        [TestInitialize()]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB");
            builder.EnableSensitiveDataLogging();

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new IngredientHandler(new IngredientRepo(context), new IngredientCategoryRelationRepo(context), new IngredientUnitTypeRelationRepo(context));
            categoryHandler = new IngredientCategoryHandler(new IngredientCategoryRepo(context), new IngredientCategoryRelationRepo(context));
            unitTypeHandler = new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context));

            var ingredients = handler.GetAll();

            if (ingredients.Any())
            {
                foreach (var ingredient in ingredients)
                {
                    handler.Delete(ingredient);
                }
            }

            category1 = new IngredientCategory() { Name = "TestCategory1" };
            category2 = new IngredientCategory() { Name = "TestCategory2" };
            categoryHandler.Create(category1);
            categoryHandler.Create(category2);

            unitType1 = new UnitType() { Name = "TestType1", AllowDecimals = false };
            unitType2 = new UnitType() { Name = "TestType2", AllowDecimals = true };
            unitTypeHandler.Create(unitType1);
            unitTypeHandler.Create(unitType2);

            ingredient = new Ingredient()
            {
                Name = "Test",
                ImageLocation = "",
                Categories = new List<IngredientCategory>() { category1 },
                UnitTypes = new List<UnitType>() { unitType1 },
                AverageWeightInKgPerUnit = 0.0,
                AverageVolumeInLiterPerUnit = 0.0,
                Deleted = false,
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            var ingredients = handler.GetAll();

            Assert.AreEqual(ingredients.Count(), 0);
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

            int newRows = handler.Create(ingredient);

            Assert.AreEqual(expectedNewRows, newRows);
        }

        public void GetByName()
        {
            Ingredient bynameIngredient = handler.GetByName(ingredient.Name);

            ingredient.Id = bynameIngredient.Id;

            Assert.AreEqual(bynameIngredient.Name, ingredient.Name);
        }

        public void GetById()
        {
            Ingredient byidIngredient = handler.GetById(ingredient.Id);

            ingredient = byidIngredient;

            Assert.AreEqual(byidIngredient.Name, ingredient.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            ingredient.Name = updatedName;

            ingredient.Categories.Add(category2);
            ingredient.UnitTypes.Add(unitType2);

            int updatedRows = handler.Update(ingredient);

            Assert.AreEqual(3, updatedRows);
            Assert.AreEqual(handler.GetById(ingredient.Id).Name, updatedName);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(ingredient);

            Assert.AreEqual(3, removedRows);

            Assert.IsNull(handler.GetById(ingredient.Id));
        }
    }
}
