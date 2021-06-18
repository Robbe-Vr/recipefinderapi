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

        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB-users", new InMemoryDatabaseRoot());
            builder.EnableSensitiveDataLogging();
            builder.ConfigureWarnings(options =>
            {
                options.Ignore(new[] { CoreEventId.ManyServiceProvidersCreatedWarning });
            });

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new UserHandler(new UserRepo(context), new UserRoleRelationRepo(context), new KitchenRepo(context));
            roleHandler = new RoleHandler(new RoleRepo(context), new UserRoleRelationRepo(context));

            role1 = new Role() { Name = "Default", Id = "75483" };
            role2 = new Role() { Name = "Admin", Id = "75484" };
            context.Roles.AddRange(new[] { role1, role2 });
            context.SaveChanges();
            role1 = context.Roles.First(x => x.Name == role1.Name);
            role2 = context.Roles.First(x => x.Name == role2.Name);

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
                Roles = new List<Role>() { role1 },
                Deleted = false,
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            Initialize();

            var users = handler.GetAll();

            Assert.AreEqual(0, users.Count());
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
            int expectedNewRows = 2;

            int newRows = handler.Create(user);

            Assert.IsTrue(newRows == expectedNewRows);
            user = handler.GetByName(user.Name);
        }

        public void GetByName()
        {
            User bynameUser = handler.GetByName(user.Name);

            Assert.AreEqual(user.Name, bynameUser.Name);
        }

        public void GetById()
        {
            User byidUser = handler.GetById(user.Id);

            Assert.AreEqual(user.Name, byidUser.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            user.Name = updatedName;
            user.Roles.Add(role2);

            int updatedRows = handler.Update(user);

            Assert.IsTrue(updatedRows > 0);
            Assert.AreEqual(updatedName, handler.GetById(user.Id).Name);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(user);

            Assert.IsTrue(removedRows > 0);

            Assert.IsNull(handler.GetById(user.Id));
        }
    }
}
