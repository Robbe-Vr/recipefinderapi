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
    public class GroceryListsController : ControllerBase
    {
        private GroceryListHandler handler;

        public GroceryListsController(GroceryListHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<GroceryListsController>
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

        // GET api/<GroceryListsController>/5
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

        [HttpGet("byuserid/{id}")]
        public IActionResult GetByUserId(string id)
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetAllByUserId(id),
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

        // POST api/<GroceryListsController>
        [HttpPost]
        public IActionResult Post([FromBody] GroceryList value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Create(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // PUT api/<GroceryListsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] GroceryList value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Update(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // DELETE api/<GroceryListsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Delete(new GroceryList() { Id = id }),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }
    }
}
