using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RecipeFinderWabApi.Logic.Handlers;
using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.Controllers
{
    [EnableCors("RFCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private RoleHandler handler;

        public RolesController(RoleHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<RolesController>
        [HttpGet]
        public IEnumerable<Role> Get()
        {
            return handler.GetAll();
        }

        // GET api/<RolesController>/5
        [HttpGet("{id}")]
        public Role Get(string id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byname/{name}")]
        public Role GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<RolesController>
        [HttpPost]
        public void Post([FromBody] Role value)
        {
            handler.Create(value);
        }

        // PUT api/<RolesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Role value)
        {
            handler.Update(value);
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            handler.Delete(new Role() { Id = id });
        }
    }
}
