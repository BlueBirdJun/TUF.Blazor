using Microsoft.AspNetCore.Authentication.JwtBearer;
using TUF.Application.Handlers.Boards;
using TUF.Database.TUFDB;
using TUF.Infrastructure.Identity.Users;
using TUF.Shared.Authorization;
using TUF.Shared.Dtos.Boards;

namespace TUF.HostApi.Controllers.Borads;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BoardController : VersionNeutralApiController
{
    private readonly IUserService _userService;
    public BoardController(IUserService userService) => _userService = userService;
    [HttpGet]
    [Route("boardinfo")]
    [AllowAnonymous]
    public async Task<BoardInfoDto> GetBoardinfo()
    {
        BoardInfoDto rt = new BoardInfoDto();
        try
        {
            GetBoardInfoHandler.Query data = new GetBoardInfoHandler.Query();
            var rtdata = await Mediator.Send(data);
            if(rtdata.Success)
            {
                rt.Success = true;
                rt.OutPutValue = rtdata.OutPutValue;
            }
            else
            {
                rt.Success = false;
                rt.Message = rtdata.Message;

            }
        }
        catch (Exception ex)        
        {
            rt.Success = false;
            rt.Message = ex.Message;
        }
        return rt;
    }

    [HttpPost]
    [Route("board")]
    
    public async Task<BoardDto> AddBoardinfo([FromBody]BoardDto.Request param)
    {
        BoardDto rt = new BoardDto();
        try
        {
            CreateUpdateHandler.Query data = new CreateUpdateHandler.Query();
            param.CreatedBy= User.GetUserId();
            data.parameter = param;
            var rtdata = await Mediator.Send(data);
            if (rtdata.Success)
            {
                rt.Success = true;
                rt.OutPutValue = rtdata.OutPutValue;
            }
            else
            {
                rt.Success = false;
                rt.Message = rtdata.Message;
            }
        }
        catch (Exception ex)
        {
            rt.Success = false;
            rt.Message = ex.Message;
        }
        return rt;
    }

    [HttpPut]
    [Route("board")]
    [AllowAnonymous]
    public async Task<BoardDto> UpdBoardinfo([FromBody] BoardDto.Request param)
    {
        BoardDto rt = new BoardDto();
        try
        {
            param.CreatedBy = User.GetUserId();
            CreateUpdateHandler.Query data = new CreateUpdateHandler.Query();
            data.parameter = param;
            var rtdata = await Mediator.Send(data);
            if (rtdata.Success)
            {
                rt.Success = true;
                rt.OutPutValue = rtdata.OutPutValue;
            }
            else
            {
                rt.Success = false;
                rt.Message = rtdata.Message;
            }
        }
        catch (Exception ex)
        {
            rt.Success = false;
            rt.Message = ex.Message;
        }
        return rt;
    }

}
