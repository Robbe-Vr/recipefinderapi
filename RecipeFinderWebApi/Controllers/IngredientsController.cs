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
    public class IngredientsController : ControllerBase
    {
        private IngredientHandler handler;

        public IngredientsController(IngredientHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<IngredientsController>
        [HttpGet]
        public IEnumerable<Ingredient> Get()
        {
            return handler.GetAll();
        }

        // GET api/<IngredientsController>/5
        [HttpGet("{id}")]
        public Ingredient Get(string id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byname/{name}")]
        public Ingredient GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<IngredientsController>
        [HttpPost]
        public void Post([FromBody] Ingredient value)
        {
            handler.Create(value);
        }

        // PUT api/<IngredientsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Ingredient value)
        {
            handler.Update(value);
        }

        // DELETE api/<IngredientsController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            handler.Delete(new Ingredient() { Id = id });
        }
    }
}
