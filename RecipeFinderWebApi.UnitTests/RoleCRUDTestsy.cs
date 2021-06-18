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
    public class RoleCRUDTests
    {
        private RoleHandler handler;

        private Role role;

        public RoleCRUDTests()
        {
        }

        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB-roles", new InMemoryDatabaseRoot());
            builder.EnableSensitiveDataLogging();
            builder.ConfigureWarnings(options =>
            {
                options.Ignore(new[] { CoreEventId.ManyServiceProvidersCreatedWarning });
            });

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new RoleHandler(new RoleRepo(context), new UserRoleRelationRepo(context));

            role = new Role()
            {
                Name = "Test",
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            Initialize();

            var roles = handler.GetAll();

            Assert.AreEqual(0, roles.Count());
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

            int newRows = handler.Create(role);

            Assert.AreEqual(expectedNewRows, newRows);
        }

        public void GetByName()
        {
            Role bynameRole = handler.GetByName(role.Name);

            Assert.AreEqual(role.Name, bynameRole.Name);
        }

        public void GetById()
        {
            Role byidRole = handler.GetById(role.Id);

            Assert.AreEqual(role.Name, byidRole.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            role.Name = updatedName;

            int updatedRows = handler.Update(role);

            Assert.AreEqual(1, updatedRows);
            Assert.AreEqual(updatedName, handler.GetById(role.Id).Name);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(role);

            Assert.AreEqual(1, removedRows);

            Assert.IsNull(handler.GetById(role.Id));
        }
    }
}
