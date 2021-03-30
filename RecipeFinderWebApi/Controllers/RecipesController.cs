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
    public class RecipesController : ControllerBase
    {
        private RecipeHandler handler;

        public RecipesController(RecipeHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<RecipesController>
        [HttpGet]
        public IEnumerable<Recipe> Get()
        {
            return handler.GetAll();
        }

        // GET api/<RecipesController>/5
        [HttpGet("{id}")]
        public Recipe Get(string id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byname/{name}")]
        public Recipe GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<RecipesController>
        [HttpPost]
        public void Post([FromBody] Recipe value)
        {
            handler.Create(value);
        }

        // PUT api/<RecipesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Recipe value)
        {
            handler.Update(value);
        }

        // DELETE api/<RecipesController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            handler.Delete(new Recipe() { Id = id });
        }
    }
}
