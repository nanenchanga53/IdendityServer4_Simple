using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Api.FrameWork.Controllers
{
    
    public class IdentityController : ApiController
    {
        [Route("Identity")]
        [Authorize]
        public IHttpActionResult Get()
        {
            var caller = User as ClaimsPrincipal;
            
            return Json(from c in caller.Claims select new { c.Type, c.Value });
        }

    }
}
