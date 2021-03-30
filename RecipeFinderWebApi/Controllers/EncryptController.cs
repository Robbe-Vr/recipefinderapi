using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeFinderWabApi.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.Controllers
{
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
        public EncryptionObject Post(EncryptionObject encObj)
        {
            return enc.HashString(encObj);
        }

        [HttpGet("getsalt")]
        public EncryptionObject CreateSalt()
        {
            return new EncryptionObject() { Result = enc.CreateSalt(8), };
        }
    }
}
