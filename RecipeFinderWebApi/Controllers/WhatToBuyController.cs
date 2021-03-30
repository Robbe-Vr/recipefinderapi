using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeFinderWabApi.Logic;
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
    public class WhatToBuyController : ControllerBase
    {
        private WhatToBuyAlgorithm algorithm;

        public WhatToBuyController(WhatToBuyAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        [HttpPost]
        public IEnumerable<KitchenIngredient> Post([FromBody] WhatToBuyFilterObject filters)
        {
            return algorithm.Calculate(filters);
        }
    }
}
