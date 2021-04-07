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
    public class RecipeCategorysController : ControllerBase
    {
        private RecipeCategoryHandler handler;

        public RecipeCategorysController(RecipeCategoryHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<RecipeCategorysController>
        [HttpGet]
        public IEnumerable<RecipeCategory> Get()
        {
            return handler.GetAll();
        }

        // GET api/<RecipeCategorysController>/5
        [HttpGet("{id}")]
        public RecipeCategory Get(int id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byname/{name}")]
        public RecipeCategory GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<RecipeCategorysController>
        [HttpPost]
        public void Post([FromBody] RecipeCategory value)
        {
            handler.Create(value);
        }

        // PUT api/<RecipeCategorysController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] RecipeCategory value)
        {
            handler.Update(value);
        }

        // DELETE api/<RecipeCategorysController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            handler.Delete(new RecipeCategory() { CountId = id });
        }
    }
}
