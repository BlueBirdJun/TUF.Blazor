using Microsoft.AspNetCore.Authentication.JwtBearer;
using NSwag.Annotations;
using TUF.Application.Identity.Roles;
using TUF.Shared.Authorization;

namespace TUF.HostApi.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolesController : VersionNeutralApiController
    {
       
        private readonly IRoleService _roleService;
        private readonly ICurrentUser _curr;
        public RolesController( CurrentUser curr)
        {
            //IRoleService roleService,
            //_roleService = roleService;
            _curr = curr;
        }

        [HttpGet]
        //[MustHavePermission(FSHAction.View, FSHResource.Roles)]
        [OpenApiOperation("Get a list of all roles.", "")]
        public Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken)
        {
            return _roleService.GetListAsync(cancellationToken);
        }
        [HttpGet("t1")]
        public async Task<IActionResult> create()
        {
            var c = _curr.GetUserId();
            var d = _curr.IsAuthenticated();
            
            return Ok("");         
        }
    }
}
