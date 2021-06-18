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
    public class IngredientCategoryCRUDTests
    {
        private IngredientCategoryHandler handler;

        private IngredientCategory category;

        public IngredientCategoryCRUDTests()
        {
        }

        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB-ingredientcategories", new InMemoryDatabaseRoot());
            builder.EnableSensitiveDataLogging();
            builder.ConfigureWarnings(options =>
            {
                options.Ignore(new[] { CoreEventId.ManyServiceProvidersCreatedWarning });
            });

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new IngredientCategoryHandler(new IngredientCategoryRepo(context), new IngredientCategoryRelationRepo(context));

            category = new IngredientCategory()
            {
                Name = "Test",
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            Initialize();

            var categories = handler.GetAll();

            Assert.AreEqual(0, categories.Count());
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
            int expectedNewRows = 1;

            int newRows = handler.Create(category);

            Assert.AreEqual(expectedNewRows, newRows);
            category = handler.GetByName(category.Name);
        }

        public void GetByName()
        {
            IngredientCategory bynameCategory = handler.GetByName(category.Name);

            Assert.AreEqual(category.Name, bynameCategory.Name);
        }

        public void GetById()
        {
            IngredientCategory byidCategory = handler.GetById(category.CountId);

            Assert.AreEqual(category.Name, byidCategory.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            category.Name = updatedName;

            int updatedRows = handler.Update(category);

            Assert.AreEqual(1, updatedRows);
            Assert.AreEqual(updatedName, handler.GetById(category.CountId).Name);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(category);

            Assert.AreEqual(1, removedRows);

            Assert.IsNull(handler.GetById(category.CountId));
        }
    }
}
