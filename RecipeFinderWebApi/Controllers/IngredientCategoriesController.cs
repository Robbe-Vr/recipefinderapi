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
    public class IngredientCategorysController : ControllerBase
    {
        private IngredientCategoryHandler handler;

        public IngredientCategorysController(IngredientCategoryHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<IngredientCategorysController>
        [HttpGet]
        public IEnumerable<IngredientCategory> Get()
        {
            return handler.GetAll();
        }

        // GET api/<IngredientCategorysController>/5
        [HttpGet("{id}")]
        public IngredientCategory Get(int id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byname/{name}")]
        public IngredientCategory GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<IngredientCategorysController>
        [HttpPost]
        public void Post([FromBody] IngredientCategory value)
        {
            handler.Create(value);
        }

        // PUT api/<IngredientCategorysController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] IngredientCategory value)
        {
            handler.Update(value);
        }

        // DELETE api/<IngredientCategorysController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            handler.Delete(new IngredientCategory() { Id = id });
        }
    }
}
