using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.DAL;
using RecipeFinderWebApi.DAL.Repositories;
using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [TestInitialize()]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB");
            builder.EnableSensitiveDataLogging();

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new GroceryListHandler(new GroceryListRepo(context));
            userHandler = new UserHandler(new UserRepo(context), new UserRoleRelationRepo(context), new KitchenRepo(context));
            RoleHandler roleHandler = new RoleHandler(new RoleRepo(context), new UserRoleRelationRepo(context));

            var groceryLists = handler.GetAll();

            if (groceryLists.Any())
            {
                foreach (var groceryList in groceryLists)
                {
                    handler.Delete(groceryList);
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
                Deleted = false,
            };
            userHandler.Create(user);
            user = userHandler.GetByName(user.Name);

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
            var groceryLists = handler.GetAll();

            Assert.AreEqual(groceryLists.Count(), 0);
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

            int newRows = handler.Create(groceryList);

            Assert.AreEqual(expectedNewRows, newRows);
        }

        public void GetByName()
        {
            GroceryList bynameGroceryList = handler.GetByName(groceryList.Name);

            groceryList.Id = bynameGroceryList.Id;

            Assert.AreEqual(bynameGroceryList.Name, groceryList.Name);
        }

        public void GetById()
        {
            GroceryList byidGroceryList = handler.GetById(groceryList.Id);

            groceryList = byidGroceryList;

            Assert.AreEqual(byidGroceryList.Name, groceryList.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            groceryList.Name = updatedName;

            int updatedRows = handler.Update(groceryList);

            Assert.AreEqual(1, updatedRows);
            Assert.AreEqual(handler.GetById(groceryList.Id).Name, updatedName);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(groceryList);

            Assert.AreEqual(1, removedRows);

            Assert.IsNull(handler.GetById(groceryList.Id));
        }
    }
}
