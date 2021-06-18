using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.DAL;
using RecipeFinderWebApi.DAL.Repositories;
using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Diagnostics;

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

        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB-kitchens", new InMemoryDatabaseRoot());
            builder.EnableSensitiveDataLogging();
            builder.ConfigureWarnings(options =>
            {
                options.Ignore(new[] { CoreEventId.ManyServiceProvidersCreatedWarning });
            });

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new KitchenHandler(new KitchenRepo(context));
            userHandler = new UserHandler(new UserRepo(context), new UserRoleRelationRepo(context), new KitchenRepo(context));
            ingredientHandler = new IngredientHandler(new IngredientRepo(context), new IngredientCategoryRelationRepo(context), new IngredientUnitTypeRelationRepo(context),
                null);
            unitTypeHandler = new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context));
            RoleHandler roleHandler = new RoleHandler(new RoleRepo(context), new UserRoleRelationRepo(context));

            Role role = new Role() { Name = "TestRole", Id = "578493" };
            context.Roles.Add(role);
            context.SaveChanges();
            user = new User()
            {
                Id = "75483",
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
                Roles = new List<Role>() { context.Roles.First(x => x.Name == role.Name) },
                Deleted = false,
            };
            context.Users.Add(user);
            context.SaveChanges();
            user = context.Users.First(x => x.Name == user.Name);

            unitType1 = new UnitType() { Name = "TestType1", AllowDecimals = false };
            unitType2 = new UnitType() { Name = "TestType2", AllowDecimals = true };
            context.UnitTypes.AddRange(new[] { unitType1, unitType2 });
            context.SaveChanges();
            unitType1 = context.UnitTypes.First(x => x.Name == unitType1.Name);
            unitType2 = context.UnitTypes.First(x => x.Name == unitType2.Name);

            ingredient = new Ingredient()
            {
                Id = "75483",
                Name = "Test",
                ImageLocation = "",
                Categories = new List<IngredientCategory>(),
                UnitTypes = new List<UnitType>() { unitType1 },
                AverageWeightInKgPerUnit = 0.0,
                AverageVolumeInLiterPerUnit = 0.0,
                Deleted = false,
            };
            context.Ingredients.AddRange(new[] { ingredient });
            context.SaveChanges();
            ingredient = context.Ingredients.First(x => x.Name == ingredient.Name);

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
            Initialize();

            var kitchens = handler.GetAll();

            Assert.AreEqual(0, kitchens.Count());
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

            int newRows = handler.Create(kitchen.Ingredients.ToArray()[0]);

            Assert.AreEqual(expectedNewRows, newRows);
            kitchen = handler.GetByUserName(user.Name);
        }

        public void GetByName()
        {
            Kitchen bynameKitchen = handler.GetByUserName(kitchen.User.Name);

            Assert.AreEqual(kitchen.UserId, bynameKitchen.UserId);
        }

        public void GetById()
        {
            Kitchen byidKitchen = handler.GetByUserId(kitchen.UserId);

            Assert.AreEqual(kitchen.User.Name, byidKitchen.User.Name);
        }

        public void Update()
        {
            double updatedUnits = 7;

            kitchen.Ingredients.ToArray()[0].Units = updatedUnits;

            int updatedRows = handler.Update(kitchen.Ingredients.ToArray()[0]);

            Assert.AreEqual(1, updatedRows);
            Assert.AreEqual(updatedUnits, handler.GetById(kitchen.Ingredients.ToArray()[0].CountId).Units);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(kitchen.Ingredients.ToArray()[0]);

            Assert.AreEqual(1, removedRows);

            Assert.IsNull(handler.GetById(kitchen.Ingredients.ToArray()[0].CountId));
        }
    }
}
