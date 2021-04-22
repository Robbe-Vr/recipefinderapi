using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeFinderWabApi.Logic.Handlers;
using RecipeFinderWebApi.DAL;
using RecipeFinderWebApi.DAL.Repositories;
using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeFinderWebApi.UnitTests
{
    [TestClass]
    public class KitchenCRUDTests
    {
        private KitchenHandler handler;
        private UserHandler userHandler;
        private IngredientHandler ingredientHandler;
        private UnitTypeHandler unitTypeHandler;

        private Kitchen kitchen;

        private User user;
        private Ingredient ingredient;

        private UnitType unitType1;
        private UnitType unitType2;

        public KitchenCRUDTests()
        {
        }

        [TestInitialize()]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB");
            builder.EnableSensitiveDataLogging();

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new KitchenHandler(new KitchenRepo(context));
            userHandler = new UserHandler(new UserRepo(context), new UserRoleRelationRepo(context), new KitchenRepo(context));
            ingredientHandler = new IngredientHandler(new IngredientRepo(context), new IngredientCategoryRelationRepo(context), new IngredientUnitTypeRelationRepo(context));
            unitTypeHandler = new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context));
            RoleHandler roleHandler = new RoleHandler(new RoleRepo(context), new UserRoleRelationRepo(context));

            var kitchens = handler.GetAll();

            if (kitchens.Any())
            {
                foreach (var kitchen in kitchens)
                {
                    handler.Delete(kitchen);
                }
            }

            Role role = new Role() { Name = "TestRole" };
            roleHandler.Create(role);
            user = new User()
            {
                Name = "Test",
                Email = "test@test.net",
                Salt = "12345",
                PasswordHashed = "!@#$%",
                DOB = DateTime.Now,
                CreationDate = DateTime.Now,
                EMAIL_NORMALIZED = "",
                NAME_NORMALIZED = "",
                ConcurrencyStamp = "",
                SecurityStamp = "",
                PhoneNumber = "",
                EmailConfirmationToken = "",
                Roles = new List<Role>() { roleHandler.GetByName(role.Name) },
                Kitchen = null,
                Deleted = false,
            };
            userHandler.Create(user);

            unitType1 = new UnitType() { Name = "TestType1", AllowDecimals = false };
            unitType2 = new UnitType() { Name = "TestType2", AllowDecimals = true };
            unitTypeHandler.Create(unitType1);
            unitTypeHandler.Create(unitType2);

            ingredient = new Ingredient()
            {
                Name = "Test",
                ImageLocation = "",
                Categories = new List<IngredientCategory>(),
                UnitTypes = new List<UnitType>() { unitType1 },
                AverageWeightInKgPerUnit = 0.0,
                AverageVolumeInLiterPerUnit = 0.0,
                Deleted = false,
            };
            ingredientHandler.Create(ingredient);

            user = userHandler.GetByName(user.Name);
            ingredient = ingredientHandler.GetByName(ingredient.Name);
            unitType1 = unitTypeHandler.GetByName(unitType1.Name);
            unitType2 = unitTypeHandler.GetByName(unitType2.Name);

            kitchen = new Kitchen()
            {
                User = user,
                UserId = user.Id,
                Ingredients = new List<KitchenIngredient>()
                    { new KitchenIngredient()
                        { Ingredient = ingredient, IngredientId = ingredient.Id, Units = 5, UnitType = unitType1, UnitTypeId = unitType1.CountId, User = user, UserId = user.Id } },
                Deleted = false,
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            var kitchens = handler.GetAll();

            Assert.AreEqual(kitchens.Count(), 0);
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

            int newRows = handler.Create(kitchen.Ingredients.ToArray()[0]);

            Assert.AreEqual(expectedNewRows, newRows);
        }

        public void GetByName()
        {
            Kitchen bynameKitchen = handler.GetByUserName(kitchen.User.Name);

            kitchen.UserId = bynameKitchen.UserId;

            Assert.AreEqual(bynameKitchen.UserId, kitchen.UserId);
        }

        public void GetById()
        {
            Kitchen byidKitchen = handler.GetByUserId(kitchen.UserId);

            kitchen = byidKitchen;

            Assert.AreEqual(byidKitchen.User.Name, kitchen.User.Name);
        }

        public void Update()
        {
            double updatedUnits = 7;

            kitchen.Ingredients.ToArray()[0].Units = updatedUnits;

            int updatedRows = handler.Update(kitchen.Ingredients.ToArray()[0]);

            Assert.AreEqual(1, updatedRows);
            Assert.AreEqual(handler.GetById(kitchen.Ingredients.ToArray()[0].CountId).Units, updatedUnits);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(kitchen.Ingredients.ToArray()[0]);

            Assert.AreEqual(1, removedRows);

            Assert.IsNull(handler.GetById(kitchen.Ingredients.ToArray()[0].CountId));
        }
    }
}
