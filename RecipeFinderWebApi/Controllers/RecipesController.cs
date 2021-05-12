using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.UI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeFinderWebApi.Logic;

namespace RecipeFinderWebApi.UI.Controllers
{
    [RequiresRoles(true, "Default")]
    [EnableCors("RFCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private RecipeHandler handler;
        private PreparableRecipesAlgorithm preparableRecipeHandler;

        public RecipesController(RecipeHandler recipeHandler, KitchenHandler kitchenHandler, UnitTypeHandler unitTypeHandler)
        {
            handler = recipeHandler;
            preparableRecipeHandler = new PreparableRecipesAlgorithm(recipeHandler, kitchenHandler, unitTypeHandler);
        }

        // GET: api/<RecipesController>
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

        [HttpGet("preparable/{userId}")]
        public IActionResult GetPreparable(string userId)
        {
            return ResponseFilter.FilterDataResponse(
                preparableRecipeHandler.GetPreparableForUser(userId),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        [HttpGet("bycook/{userId}")]
        public IActionResult GetAllFromCook(string userId)
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetAllByCook(userId),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // GET api/<RecipesController>/5
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

        // POST api/<RecipesController>
        [HttpPost]
        public IActionResult Post([FromBody] RecipeWithRequirements value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Create(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // PUT api/<RecipesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RecipeWithRequirements value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Update(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // DELETE api/<RecipesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Delete(new RecipeWithRequirements() { Id = id }),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }
    }
}
