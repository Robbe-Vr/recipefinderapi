using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeFinderWebApi.Logic;
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
    public class EncryptController : ControllerBase
    {
        private Encryption enc;

        public EncryptController()
        {
            enc = new Encryption();
        }

        [HttpPost]
        public IActionResult Post(EncryptionObject encObj)
        {
            return ResponseFilter.FilterActionResponse(
                enc.HashString(encObj),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }

        [HttpGet("getsalt")]
        public IActionResult CreateSalt()
        {
            return ResponseFilter.FilterDataResponse(
                new EncryptionObject() { Result = enc.CreateSalt(8), },
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }
    }
}
