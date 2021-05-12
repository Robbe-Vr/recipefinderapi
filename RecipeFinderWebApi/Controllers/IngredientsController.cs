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
    public class IngredientsController : ControllerBase
    {
        private IngredientHandler handler;

        public IngredientsController(IngredientHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<IngredientsController>
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

        // GET api/<IngredientsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetById(id),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        [HttpGet("byname/{name}")]
        public IActionResult GetByName(string name)
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetByName(name),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // POST api/<IngredientsController>
        [HttpPost]
        public IActionResult Post([FromBody] Ingredient value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Create(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        [HttpPost("getid")]
        public IActionResult CreateGetId([FromBody] Ingredient value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.CreateGetId(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // PUT api/<IngredientsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Ingredient value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Update(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // DELETE api/<IngredientsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Delete(new Ingredient() { Id = id }),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }
    }
}
