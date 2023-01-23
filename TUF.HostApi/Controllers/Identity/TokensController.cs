using NSwag.Annotations;
using TUF.HostApi.Authentication;
using TUF.Infrastructure.Identity.Tokens;

namespace TUF.HostApi.Controllers.Identity;

public class TokensController : VersionNeutralApiController
{
    private readonly ITokenService _tokenService;

    public TokensController(ITokenService tokenService) => _tokenService = tokenService;

    //[HttpGet]
    //[Route("Login")]
    //[AllowAnonymous]
    //public ActionResult<UserSession> Login()
    //{
    //    var jwtAuthenticationManager = new JwtAuthernticationManager();
    //    var userSession = jwtAuthenticationManager.GenerateJwtToken("","");
    //    if (userSession is null)
    //        return Unauthorized();
    //    else
    //        return userSession;

    //}

   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [OpenApiOperation("Request an access token using credentials.", "")]    
    public Task<TokenResponse> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken)
    {
        return _tokenService.GetTokenAsync(request, GetIpAddress(), cancellationToken);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]

    [OpenApiOperation("Request an access token using a refresh token.", "")]
    [ApiConventionMethod(typeof(TUFApiConventions), nameof(TUFApiConventions.Search))]
    public Task<TokenResponse> RefreshAsync(RefreshTokenRequest request)
    {
        return _tokenService.RefreshTokenAsync(request, GetIpAddress());
    }
   

    private string GetIpAddress() =>
    Request.Headers.ContainsKey("X-Forwarded-For")
        ? Request.Headers["X-Forwarded-For"]
        : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";


     
}
