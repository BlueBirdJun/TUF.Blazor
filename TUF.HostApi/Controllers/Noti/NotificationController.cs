namespace TUF.HostApi.Controllers.Noti;

public class NotificationController : VersionNeutralApiController
{
    [HttpGet]
    [Route("T1")]
    [AllowAnonymous]
    public  Task<IActionResult>  NotiSender()
    { 
        return Task.FromResult<IActionResult>(Ok("AA"));
    }
}
