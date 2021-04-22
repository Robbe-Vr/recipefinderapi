using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeFinderWabApi.Logic.Handlers;
using RecipeFinderWebApi.DAL;
using RecipeFinderWebApi.DAL.Repositories;
using RecipeFinderWebApi.Exchange.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RecipeFinderWebApi.UnitTests
{
    [TestClass]
    public class RecipeCategoryCRUDTests
    {
        private RecipeCategoryHandler handler;

        private RecipeCategory category;

        public RecipeCategoryCRUDTests()
        {
        }

        [TestInitialize()]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB");
            builder.EnableSensitiveDataLogging();

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new RecipeCategoryHandler(new RecipeCategoryRepo(context), new RecipeCategoryRelationRepo(context));

            var categories = handler.GetAll();

            if (categories.Any())
            {
                foreach (var category in categories)
                {
                    handler.Delete(category);
                }
            }

            category = new RecipeCategory()
            {
                Name = "Test",
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            var categories = handler.GetAll();

            Assert.AreEqual(categories.Count(), 0);
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
            int expectedNewRows = 1;

            int newRows = handler.Create(category);

            Assert.AreEqual(expectedNewRows, newRows);
        }

        public void GetByName()
        {
            RecipeCategory bynameCategory = handler.GetByName(category.Name);

            category.CountId = bynameCategory.CountId;

            Assert.AreEqual(bynameCategory.Name, category.Name);
        }

        public void GetById()
        {
            RecipeCategory byidCategory = handler.GetById(category.CountId);

            category = byidCategory;

            Assert.AreEqual(byidCategory.Name, category.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            category.Name = updatedName;

            int updatedRows = handler.Update(category);

            Assert.AreEqual(1, updatedRows);
            Assert.AreEqual(handler.GetById(category.CountId).Name, updatedName);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(category);

            Assert.AreEqual(1, removedRows);

            Assert.IsNull(handler.GetById(category.CountId));
        }
    }
}
