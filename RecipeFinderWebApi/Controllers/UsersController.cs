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
        public ActionResult<IEnumerable<User>> Get()
        {
            return handler.GetAll().ToArray();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(string id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byname/{name}")]
        public ActionResult<User> GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<int> Post([FromBody] User value)
        {
            return handler.Create(value);
        }

        // POST api/<UsersController>/getid
        [HttpPost]
        public ActionResult<User> CreateGetById([FromBody] User value)
        {
            return handler.CreateGetId(value);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public ActionResult<int> Put(int id, [FromBody] User value)
        {
            return handler.Update(value);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public ActionResult<int> Delete(string id)
        {
            return handler.Delete(new User() { Id = id });
        }
    }
}
