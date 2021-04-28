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
        public IActionResult Get()
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetAll(),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // GET api/<IngredientCategorysController>/5
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

        // POST api/<IngredientCategorysController>
        [HttpPost]
        public IActionResult Post([FromBody] IngredientCategory value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Create(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // PUT api/<IngredientCategorysController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] IngredientCategory value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Update(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // DELETE api/<IngredientCategorysController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Delete(new IngredientCategory() { CountId = id }),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }
    }
}
