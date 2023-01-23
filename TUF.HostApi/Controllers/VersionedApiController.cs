using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TUF.HostApi.Controllers;


[Route("api/v{version:apiVersion}/[controller]")]

public class VersionedApiController : BaseApiController
{
}
