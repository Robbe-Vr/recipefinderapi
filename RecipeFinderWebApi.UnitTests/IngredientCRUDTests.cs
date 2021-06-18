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

        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB-ingrdients", new InMemoryDatabaseRoot());
            builder.EnableSensitiveDataLogging();
            builder.ConfigureWarnings(options =>
            {
                options.Ignore(new[] { CoreEventId.ManyServiceProvidersCreatedWarning });
            });

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new IngredientHandler(new IngredientRepo(context), new IngredientCategoryRelationRepo(context), new IngredientUnitTypeRelationRepo(context),
                null);
            categoryHandler = new IngredientCategoryHandler(new IngredientCategoryRepo(context), new IngredientCategoryRelationRepo(context));
            unitTypeHandler = new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context));

            category1 = new IngredientCategory() { Name = "TestCategory1" };
            category2 = new IngredientCategory() { Name = "TestCategory2" };
            context.IngredientCategories.AddRange(new[] { category1, category2 });
            context.SaveChanges();
            category1 = context.IngredientCategories.First(x => x.Name == category1.Name);
            category2 = context.IngredientCategories.First(x => x.Name == category2.Name);

            unitType1 = new UnitType() { Name = "TestType1", AllowDecimals = false };
            unitType2 = new UnitType() { Name = "TestType2", AllowDecimals = true };
            context.UnitTypes.AddRange(new[] { unitType1, unitType2 });
            context.SaveChanges();
            unitType1 = context.UnitTypes.First(x => x.Name == unitType1.Name);
            unitType2 = context.UnitTypes.First(x => x.Name == unitType2.Name);

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
            Initialize();

            var ingredients = handler.GetAll();

            Assert.AreEqual(0, ingredients.Count());
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

            int newRows = handler.Create(ingredient);

            Assert.AreEqual(expectedNewRows, newRows);
            ingredient = handler.GetByName(ingredient.Name);
        }

        public void GetByName()
        {
            Ingredient bynameIngredient = handler.GetByName(ingredient.Name);

            Assert.AreEqual(ingredient.Name, bynameIngredient.Name);
        }

        public void GetById()
        {
            Ingredient byidIngredient = handler.GetById(ingredient.Id);

            Assert.AreEqual(ingredient.Name, byidIngredient.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            ingredient.Name = updatedName;

            ingredient.Categories.Add(category2);
            ingredient.UnitTypes.Add(unitType2);

            int updatedRows = handler.Update(ingredient);

            Assert.AreEqual(3, updatedRows);
            Assert.AreEqual(updatedName, handler.GetById(ingredient.Id).Name);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(ingredient);

            Assert.AreEqual(3, removedRows);

            Assert.IsNull(handler.GetById(ingredient.Id));
        }
    }
}
