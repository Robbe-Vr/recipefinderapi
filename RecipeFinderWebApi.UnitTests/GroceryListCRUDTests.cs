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
    public class GroceryListCRUDTests
    {
        private GroceryListHandler handler;
        private UserHandler userHandler;

        private GroceryList groceryList;
        private User user;

        public GroceryListCRUDTests()
        {
        }

        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB-grocerylists", new InMemoryDatabaseRoot());
            builder.EnableSensitiveDataLogging();
            builder.ConfigureWarnings(options =>
            {
                options.Ignore(new[] { CoreEventId.ManyServiceProvidersCreatedWarning });
            });

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new GroceryListHandler(new GroceryListRepo(context));
            userHandler = new UserHandler(new UserRepo(context), new UserRoleRelationRepo(context), new KitchenRepo(context));
            RoleHandler roleHandler = new RoleHandler(new RoleRepo(context), new UserRoleRelationRepo(context));

            Role role = new Role() { Name = "TestRole", Id = "75483" };
            context.Roles.AddRange(new[] { role });
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
            context.Users.AddRange(new[] { user });
            context.SaveChanges();
            user = context.Users.First(x => x.Name == user.Name);

            groceryList = new GroceryList()
            {
                Name = "Test",
                Value = "1 - Test1 - Test2 | 2 - Test3 - Test4",
                User = user,
                UserId = user.Id,
                Deleted = false,
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            Initialize();

            var groceryLists = handler.GetAll();

            Assert.AreEqual(0, groceryLists.Count());
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

            int newRows = handler.Create(groceryList);

            Assert.AreEqual(expectedNewRows, newRows);
            groceryList = handler.GetByName(groceryList.Name);
        }

        public void GetByName()
        {
            GroceryList bynameGroceryList = handler.GetByName(groceryList.Name);

            Assert.AreEqual(bynameGroceryList.Name, groceryList.Name);
        }

        public void GetById()
        {
            GroceryList byidGroceryList = handler.GetById(groceryList.Id);

            Assert.AreEqual(groceryList.Name, byidGroceryList.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            groceryList.Name = updatedName;

            int updatedRows = handler.Update(groceryList);

            Assert.AreEqual(1, updatedRows);
            Assert.AreEqual(updatedName, handler.GetById(groceryList.Id).Name);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(groceryList);

            Assert.AreEqual(1, removedRows);

            Assert.IsNull(handler.GetById(groceryList.Id));
        }
    }
}
