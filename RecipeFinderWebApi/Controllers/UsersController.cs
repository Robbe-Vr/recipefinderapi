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
    public class UsersController : ControllerBase
    {
        private UserHandler handler;

        public UsersController(UserHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return handler.GetAll();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public User Get(string id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byname/{name}")]
        public User GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] User value)
        {
            handler.Create(value);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User value)
        {
            handler.Update(value);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            handler.Delete(new User() { Id = id });
        }
    }
}
