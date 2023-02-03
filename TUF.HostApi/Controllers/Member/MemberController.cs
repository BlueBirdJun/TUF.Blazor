using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TUF.HostApi.Controllers.Member;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MemberController
{
}
