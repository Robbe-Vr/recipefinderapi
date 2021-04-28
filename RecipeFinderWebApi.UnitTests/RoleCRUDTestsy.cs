using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.DAL;
using RecipeFinderWebApi.DAL.Repositories;
using RecipeFinderWebApi.Exchange.DTOs;
using System.Collections.Generic;
using System.Linq;

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

        [TestInitialize()]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            builder.UseInMemoryDatabase("RecipeFinderDB");
            builder.EnableSensitiveDataLogging();

            RecipeFinderDbContext context = new RecipeFinderDbContext(builder.Options);

            handler = new RoleHandler(new RoleRepo(context), new UserRoleRelationRepo(context));

            var roles = handler.GetAll();

            if (roles.Any())
            {
                foreach (var role in roles)
                {
                    handler.Delete(role);
                }
            }

            role = new Role()
            {
                Name = "Test",
            };
        }

        [TestMethod]
        public void TestGetAll()
        {
            var roles = handler.GetAll();

            Assert.AreEqual(roles.Count(), 0);
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

            int newRows = handler.Create(role);

            Assert.AreEqual(expectedNewRows, newRows);
        }

        public void GetByName()
        {
            Role bynameRole = handler.GetByName(role.Name);

            role.CountId = bynameRole.CountId;

            Assert.AreEqual(bynameRole.Name, role.Name);
        }

        public void GetById()
        {
            Role byidRole = handler.GetById(role.Id);

            role = byidRole;

            Assert.AreEqual(byidRole.Name, role.Name);
        }

        public void Update()
        {
            string updatedName = "TestUpdate";

            role.Name = updatedName;

            int updatedRows = handler.Update(role);

            Assert.AreEqual(1, updatedRows);
            Assert.AreEqual(handler.GetById(role.Id).Name, updatedName);
        }

        public void Delete()
        {
            int removedRows = handler.Delete(role);

            Assert.AreEqual(1, removedRows);

            Assert.IsNull(handler.GetById(role.Id));
        }
    }
}
