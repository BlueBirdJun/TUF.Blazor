using Microsoft.AspNetCore.Authentication.JwtBearer;
using TUF.Application.Handlers.Commons;
using TUF.Shared.Dtos.Boards;

namespace TUF.HostApi.Controllers.Commons;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommonController : VersionNeutralApiController
{
    [HttpGet]
    [Route("seed")]
    [AllowAnonymous]
    public async Task<BoardInfoDto> addseed()
    {
        BoardInfoDto rt = new BoardInfoDto();
        try
        {
           SeedHandler.Query data = new SeedHandler.Query();
           var rtdata = await Mediator.Send(data);
        }
        catch (Exception ex)
        {
            rt.Success = false;
            rt.Message = ex.Message;
        }
        return rt;
    }
}
