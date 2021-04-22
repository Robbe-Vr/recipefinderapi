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
    public class UserCRUDTests
    {
        private UserHandler handler;

        private RoleHandler roleHandler;

        private User user;

        private Role role1;
        private Role role2;

        public UserCRUDTests()
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB");
            builder.EnableSensitiveDataLogging();

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new UserHandler(new UserRepo(context), new UserRoleRelationRepo(context), new KitchenRepo(context));
            roleHandler = new RoleHandler(new RoleRepo(context), new UserRoleRelationRepo(context));

            var users = handler.GetAll();

            if (users.Any())
            {
                foreach (var user in users)
                {
                    handler.Delete(user);
                }
            }

            role1 = new Role() { Name = "Default" };
            role2 = new Role() { Name = "Admin" };
            roleHandler.Create(role1);
            roleHandler.Create(role2);

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
                Roles = new List<Role>() { roleHandler.GetByName(role1.Name) },
                Kitchen = null,
                Deleted = false,
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            var users = handler.GetAll();

            Assert.AreEqual(users.Count(), 0);
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
            int expectedNewRows = 2;

            int newRows = handler.Create(user);

            Assert.IsTrue(newRows == expectedNewRows);
        }

        public void GetByName()
        {
            User bynameUser = handler.GetByName(user.Name);

            user.Id = bynameUser.Id;

            Assert.AreEqual(bynameUser.Name, user.Name);
        }

        public void GetById()
        {
            User byidUser = handler.GetById(user.Id);

            user = byidUser;

            Assert.AreEqual(byidUser.Name, user.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            user.Name = updatedName;
            user.Roles.Add(roleHandler.GetByName(role2.Name));

            int updatedRows = handler.Update(user);

            Assert.IsTrue(updatedRows > 0);
            Assert.AreEqual(handler.GetById(user.Id).Name, updatedName);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(user);

            Assert.IsTrue(removedRows > 0);

            Assert.IsNull(handler.GetById(user.Id));
        }
    }
}
