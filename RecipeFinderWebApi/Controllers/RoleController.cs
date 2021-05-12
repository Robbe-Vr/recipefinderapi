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
    public class RolesController : ControllerBase
    {
        private RoleHandler handler;

        public RolesController(RoleHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<RolesController>
        [RequiresRoles(true, "Admin")]
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

        // GET api/<RolesController>/5
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

        // POST api/<RolesController>
        [HttpPost]
        public IActionResult Post([FromBody] Role value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Create(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // PUT api/<RolesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Role value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Update(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Delete(new Role() { Id = id }),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }
    }
}
