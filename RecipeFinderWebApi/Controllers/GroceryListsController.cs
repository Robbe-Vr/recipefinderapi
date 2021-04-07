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
    public class GroceryListsController : ControllerBase
    {
        private GroceryListHandler handler;

        public GroceryListsController(GroceryListHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<GroceryListsController>
        [HttpGet]
        public ActionResult<IEnumerable<GroceryList>> Get()
        {
            return Ok(handler.GetAll());
            //return StatusCode(418, new { Error = "Error occured." });
        }

        // GET api/<GroceryListsController>/5
        [HttpGet("{id}")]
        public GroceryList Get(string id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byuserid/{id}")]
        public IEnumerable<GroceryList> GetByUserId(string id)
        {
            return handler.GetAllByUserId(id);
        }

        [HttpGet("byname/{name}")]
        public GroceryList GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<GroceryListsController>
        [HttpPost]
        public void Post([FromBody] GroceryList value)
        {
            handler.Create(value);
        }

        // PUT api/<GroceryListsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] GroceryList value)
        {
            handler.Update(value);
        }

        // DELETE api/<GroceryListsController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            handler.Delete(new GroceryList() { Id = id });
        }
    }
}
