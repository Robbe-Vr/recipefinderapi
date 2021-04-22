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
    public class UnitTypeCRUDTests
    {
        private UnitTypeHandler handler;

        private UnitType unitType;

        public UnitTypeCRUDTests()
        {
        }

        [TestInitialize()]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB");
            builder.EnableSensitiveDataLogging();

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context));

            var unitTypes = handler.GetAll();

            if (unitTypes.Any())
            {
                foreach (var unitType in unitTypes)
                {
                    handler.Delete(unitType);
                }
            }

            unitType = new UnitType()
            {
                Name = "Test",
                AllowDecimals = true,
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            var unitTypes = handler.GetAll();

            Assert.AreEqual(unitTypes.Count(), 0);
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

            int newRows = handler.Create(unitType);

            Assert.AreEqual(expectedNewRows, newRows);
        }

        public void GetByName()
        {
            UnitType bynameUnitType = handler.GetByName(unitType.Name);

            unitType.CountId = bynameUnitType.CountId;

            Assert.AreEqual(bynameUnitType.Name, unitType.Name);
        }

        public void GetById()
        {
            UnitType byidUnitType = handler.GetById(unitType.CountId);

            unitType = byidUnitType;

            Assert.AreEqual(byidUnitType.Name, unitType.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            unitType.Name = updatedName;
            unitType.AllowDecimals = !unitType.AllowDecimals;

            int updatedRows = handler.Update(unitType);

            Assert.AreEqual(1, updatedRows);
            Assert.AreEqual(handler.GetById(unitType.CountId).Name, updatedName);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(unitType);

            Assert.AreEqual(1, removedRows);

            Assert.IsNull(handler.GetById(unitType.CountId));
        }
    }
}
