using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using TUF.Infrastructure.Auth.Permissions;

namespace TUF.Api.Controllers.Test
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestController: VersionedApiController
    {

        private ICurrentUser icurr;
        public TestController(ICurrentUser _icurr)
        {
            icurr= _icurr;
        }

        [HttpGet]
        
        [MustHavePermission(TUFAction.View, TUFResource.TEST)]
        public IActionResult GetUserinfo()
        {
            var ccc = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            return Ok("");
        }


    }
}
