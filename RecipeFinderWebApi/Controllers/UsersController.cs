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
    public class UsersController : ControllerBase
    {
        private UserHandler handler;
        private UserActionHandler userActionHandler;

        public UsersController(UserHandler userHandler, UserActionHandler userActionHandler)
        {
            handler = userHandler;

            this.userActionHandler = userActionHandler;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IActionResult Get()
        {
            return ResponseFilter.FilterDataResponse(
                handler.GetAll().ToArray(),
                (int code, object obj) => { return StatusCode(code, obj);
                }
            );
        }

        // GET api/<UsersController>/5
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

        [HttpGet("actions")]
        public IActionResult GetActions()
        {
            return ResponseFilter.FilterDataResponse(
                userActionHandler.GetAll().ToArray(),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        [HttpGet("{id}/actions")]
        public IActionResult GetActionsByUserId(string id)
        {
            int countId = -1;
            if (int.TryParse(id, out countId))
            {
                return ResponseFilter.FilterDataResponse(
                    userActionHandler.GetAllByUserId(countId).ToArray(),
                    (int code, object obj) => {
                        return StatusCode(code, obj);
                    }
                );
            }
            else
            {
                return ResponseFilter.FilterDataResponse(
                    userActionHandler.GetAllByUserId(id).ToArray(),
                    (int code, object obj) => {
                        return StatusCode(code, obj);
                    }
                );
            }
        }

        // POST api/<UsersController>
        [HttpPost]
        public IActionResult Post([FromBody] User value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Create(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // POST api/<UsersController>/getid
        [HttpPost]
        public IActionResult CreateGetById([FromBody] User value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.CreateGetId(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User value)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Update(value),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return ResponseFilter.FilterActionResponse(
                handler.Delete(new User() { Id = id }),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }
    }
}
