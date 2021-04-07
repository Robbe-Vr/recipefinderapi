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
    public class IngredientCRUDTests
    {
        private IngredientHandler handler;

        private Ingredient ingredient;

        public IngredientCRUDTests()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB");
            builder.EnableSensitiveDataLogging();

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new IngredientHandler(new IngredientRepo(context), new IngredientCategoryRelationRepo(context), new IngredientUnitTypeRelationRepo(context));

            var ingredients = handler.GetAll();

            if (ingredients.Any())
            {
                foreach (var ingredient in ingredients)
                {
                    handler.Delete(ingredient);
                }
                ingredients = null;
            }

            ingredient = new Ingredient()
            {
                Name = "Test",
                ImageLocation = "",
                Categories = new List<IngredientCategory>(),
                UnitTypes = new List<UnitType>(),
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
            int expectedNewRows = 1;

            int newRows = handler.Create(ingredient);

            Assert.IsTrue(newRows == expectedNewRows);
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

            int updatedRows = handler.Update(ingredient);

            Assert.IsTrue(updatedRows > 0);
            Assert.AreEqual(handler.GetById(ingredient.Id).Name, updatedName);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(ingredient);

            Assert.IsTrue(removedRows > 0);

            Assert.IsNull(handler.GetById(ingredient.Id));
        }
    }
}
