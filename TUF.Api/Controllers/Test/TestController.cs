using TUF.Infrastructure.Auth.Permissions;

namespace TUF.Api.Controllers.Test
{
    
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
            return Ok("");
        }
    }
}
