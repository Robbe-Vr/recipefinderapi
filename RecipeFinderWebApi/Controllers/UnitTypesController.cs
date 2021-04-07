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
    public class UnitTypesController : ControllerBase
    {
        private UnitTypeHandler handler;

        public UnitTypesController(UnitTypeHandler ingredientHandler)
        {
            handler = ingredientHandler;
        }

        // GET: api/<UnitTypesController>
        [HttpGet]
        public IEnumerable<UnitType> Get()
        {
            return handler.GetAll();
        }

        // GET api/<UnitTypesController>/5
        [HttpGet("{id}")]
        public UnitType Get(int id)
        {
            return handler.GetById(id);
        }

        [HttpGet("byname/{name}")]
        public UnitType GetByName(string name)
        {
            return handler.GetByName(name);
        }

        // POST api/<UnitTypesController>
        [HttpPost]
        public void Post([FromBody] UnitType value)
        {
            handler.Create(value);
        }

        // PUT api/<UnitTypesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UnitType value)
        {
            handler.Update(value);
        }

        // DELETE api/<UnitTypesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            handler.Delete(new UnitType() { CountId = id });
        }
    }
}
