using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RecipeFinderWabApi.Logic.Handlers;
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
    public class RecipeCategorysController : ControllerBase
    {
        private RecipeCategoryHandler handler;

        public RecipeCategorysController(RecipeCategoryHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<RecipeCategorysController>
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

        // GET api/<RecipeCategorysController>/5
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

        // POST api/<RecipeCategorysController>
        [HttpPost]
        public IActionResult Post([FromBody] RecipeCategory value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Create(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // PUT api/<RecipeCategorysController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RecipeCategory value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Update(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // DELETE api/<RecipeCategorysController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Delete(new RecipeCategory() { CountId = id }),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }
    }
}
