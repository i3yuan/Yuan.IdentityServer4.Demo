using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yuan.APi.Demo.Controller
{
    [Route("api/[Controller]")]
    [ApiController]
    public class IdentityController:ControllerBase
    {
        [HttpGet("getUserClaims")]
        [Authorize]
        //[Authorize(Roles ="admin")]
        public IActionResult GetUserClaims()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
