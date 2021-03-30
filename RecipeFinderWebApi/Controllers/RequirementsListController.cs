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
    public class RequirementsListsController : ControllerBase
    {
        private RequirementsListHandler handler;

        public RequirementsListsController(RequirementsListHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<RequirementsListsController>
        [HttpGet]
        public IEnumerable<RequirementsListIngredient> Get()
        {
            return handler.GetAll();
        }

        // GET api/<RequirementsListsController>/5
        [HttpGet("{id}")]
        public RequirementsList Get(string id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byname/{name}")]
        public RequirementsList GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<RequirementsListsController>
        [HttpPost]
        public void Post([FromBody] RequirementsListIngredient value)
        {
            handler.Create(value);
        }

        // PUT api/<RequirementsListsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] RequirementsListIngredient value)
        {
            handler.Update(value);
        }

        // DELETE api/<RequirementsListsController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            handler.Delete(new RequirementsListIngredient() { RecipeId = id });
        }
    }
}
