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
        public KitchenIngredient Get(int id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byuserid/{id}")]
        public Kitchen GetByUserId(string id)
        {
            return handler.GetByUserId(id);
        }

        [HttpGet("byusername/{name}")]
        public Kitchen GetByUserName(string name)
        {
            return handler.GetByUserName(name);
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
            value.CountId = id;

            handler.Update(value);
        }

        // PUT api/<KitchensController>/5
        [HttpPut("{userId}/{ingredientId}")]
        public void Put(string userId, string ingredientId, [FromBody] KitchenIngredient value)
        {
            value.UserId = userId;
            value.IngredientId = ingredientId;

            handler.Update(value);
        }

        // DELETE api/<KitchensController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            handler.Delete(new KitchenIngredient() { CountId = id });
        }

        // DELETE api/<KitchensController>/5/5
        [HttpDelete("{userId}/{ingredientId}")]
        public void Delete(string userId, string ingredientId)
        {
            handler.Delete(new KitchenIngredient() { UserId = userId, IngredientId = ingredientId });
        }
    }
}
