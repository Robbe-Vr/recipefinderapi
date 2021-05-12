using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.UI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.Controllers
{
    [RequiresRoles(true, "Default")]
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
        public IActionResult Get()
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetAll(),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // GET api/<KitchensController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetById(id),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        [HttpGet("byuserid/{id}")]
        public IActionResult GetByUserId(string id)
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetByUserId(id),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        [HttpGet("byusername/{name}")]
        public IActionResult GetByUserName(string name)
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetByUserName(name),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // POST api/<KitchensController>
        [HttpPost]
        public IActionResult Post([FromBody] KitchenIngredient value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Create(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // PUT api/<KitchensController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] KitchenIngredient value)
        {
            value.CountId = id;

            return ResponseFilter.FilterActionResponse(
                handler.Update(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // PUT api/<KitchensController>/5
        [HttpPut("{userId}/{ingredientId}")]
        public IActionResult Put(string userId, string ingredientId, [FromBody] KitchenIngredient value)
        {
            value.UserId = userId;
            value.IngredientId = ingredientId;

            return ResponseFilter.FilterActionResponse(
                handler.Update(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // DELETE api/<KitchensController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Delete(new KitchenIngredient() { CountId = id }),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // DELETE api/<KitchensController>/5/5
        [HttpDelete("{userId}/{ingredientId}")]
        public IActionResult Delete(string userId, string ingredientId)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Delete(new KitchenIngredient() { UserId = userId, IngredientId = ingredientId }),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }
    }
}
