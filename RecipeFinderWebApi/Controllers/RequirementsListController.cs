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
    public class RequirementsListsController : ControllerBase
    {
        private RequirementsListHandler handler;

        public RequirementsListsController(RequirementsListHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<RequirementsListsController>
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

        // GET api/<RequirementsListsController>/5
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

        [HttpGet("byrecipeid/{id}")]
        public IActionResult GetByRecipeId(string id)
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetByRecipeId(id),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        [HttpGet("byrecipename/{name}")]
        public IActionResult GetByRecipeName(string name)
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetByRecipeName(name),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // POST api/<RequirementsListsController>
        [HttpPost]
        public IActionResult Post([FromBody] RequirementsListIngredient value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Create(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // PUT api/<RequirementsListsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RequirementsListIngredient value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Update(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // DELETE api/<RequirementsListsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Delete(new RequirementsListIngredient() { RecipeId = id }),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }
    }
}
