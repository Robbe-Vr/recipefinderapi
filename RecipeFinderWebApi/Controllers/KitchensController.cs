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
    public class KitchensController : ControllerBase
    {
        private KitchenHandler handler;

        public KitchensController(KitchenHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<KitchensController>
        [HttpGet]
        public IEnumerable<KitchenIngredient> Get()
        {
            return handler.GetAll();
        }

        // GET api/<KitchensController>/5
        [HttpGet("{id}")]
        public Kitchen Get(string id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byname/{name}")]
        public Kitchen GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<KitchensController>
        [HttpPost]
        public void Post([FromBody] KitchenIngredient value)
        {
            handler.Create(value);
        }

        // PUT api/<KitchensController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] KitchenIngredient value)
        {
            handler.Update(value);
        }

        // DELETE api/<KitchensController>/5
        [HttpDelete("{userId}/{ingredientId}")]
        public void Delete(string userId, string ingredientId)
        {
            handler.Delete(new KitchenIngredient() { UserId = userId, IngredientId = ingredientId });
        }
    }
}
