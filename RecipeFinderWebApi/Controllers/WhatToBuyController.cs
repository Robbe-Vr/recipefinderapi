using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeFinderWebApi.Logic;
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
    public class WhatToBuyController : ControllerBase
    {
        private WhatToBuyAlgorithm algorithm;

        public WhatToBuyController(WhatToBuyAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        [HttpPost]
        public IActionResult Post([FromBody] WhatToBuyFilterObject filters)
        {
            return ResponseFilter.FilterActionResponse(
                algorithm.Calculate(filters),
                (int code, object obj) => {
                    return StatusCode(code, obj);
                }
            );
        }
    }
}
