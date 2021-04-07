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
        public ActionResult<IEnumerable<Ingredient>> Get()
        {
            return handler.GetAll().ToArray();
        }

        // GET api/<IngredientsController>/5
        [HttpGet("{id}")]
        public ActionResult<Ingredient> Get(string id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byname/{name}")]
        public ActionResult<Ingredient> GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<IngredientsController>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Ingredient value)
        {
            return handler.Create(value);
        }

        [HttpPost("getid")]
        public ActionResult<Ingredient> CreateGetId([FromBody] Ingredient value)
        {
            return handler.CreateGetId(value);
        }

        // PUT api/<IngredientsController>/5
        [HttpPut("{id}")]
        public ActionResult<int> Put(string id, [FromBody] Ingredient value)
        {
            return handler.Update(value);
        }

        // DELETE api/<IngredientsController>/5
        [HttpDelete("{id}")]
        public ActionResult<int> Delete(string id)
        {
            return handler.Delete(new Ingredient() { Id = id });
        }
    }
}
