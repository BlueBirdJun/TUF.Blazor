using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using TUF.Infrastructure.Auth.Permissions;
using TUF.Shared.Authorization;

namespace TUF.HostApi.Controllers.Test
{
    //[ApiController]
    //[Route("[controller]")]
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestController : VersionedApiController
    {
        [HttpGet]
        //[MustHavePermission(TUFAction.View, TUFResource.TEST)]
        public IActionResult Get()
        {
            var ccc = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            return Ok("ff");
        }
    }
}
