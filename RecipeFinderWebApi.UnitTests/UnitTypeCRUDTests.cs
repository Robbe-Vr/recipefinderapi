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
    public class UnitTypeCRUDTests
    {
        private UnitTypeHandler handler;

        private UnitType unitType;

        public UnitTypeCRUDTests()
        {
        }

        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB-unittypes", new InMemoryDatabaseRoot());
            builder.EnableSensitiveDataLogging();
            builder.ConfigureWarnings(options =>
            {
                options.Ignore(new[] { CoreEventId.ManyServiceProvidersCreatedWarning });
            });

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context));

            unitType = new UnitType()
            {
                Name = "Test",
                AllowDecimals = true,
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            Initialize();

            var unitTypes = handler.GetAll();

            Assert.AreEqual(0, unitTypes.Count());
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

            int newRows = handler.Create(unitType);

            Assert.AreEqual(expectedNewRows, newRows);
        }

        public void GetByName()
        {
            UnitType bynameUnitType = handler.GetByName(unitType.Name);

            Assert.AreEqual(unitType.Name, bynameUnitType.Name);
        }

        public void GetById()
        {
            UnitType byidUnitType = handler.GetById(unitType.CountId);

            Assert.AreEqual(unitType.Name, byidUnitType.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            unitType.Name = updatedName;
            unitType.AllowDecimals = !unitType.AllowDecimals;

            int updatedRows = handler.Update(unitType);

            Assert.AreEqual(1, updatedRows);
            Assert.AreEqual(updatedName, handler.GetById(unitType.CountId).Name);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(unitType);

            Assert.AreEqual(1, removedRows);

            Assert.IsNull(handler.GetById(unitType.CountId));
        }
    }
}
