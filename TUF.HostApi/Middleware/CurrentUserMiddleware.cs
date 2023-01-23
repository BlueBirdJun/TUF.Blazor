using System.Security.Claims;
using TUF.Shared.Authorization;

namespace TUF.HostApi.Middleware;

public class CurrentUserMiddleware : IMiddleware
{
    private readonly ICurrentUserInitializer _currentUserInitializer;

    public CurrentUserMiddleware(ICurrentUserInitializer currentUserInitializer) =>
        _currentUserInitializer = currentUserInitializer;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _currentUserInitializer.SetCurrentUser(context.User);

        //context.User.Identitie
        //var ccc = context.User.FindFirstValue(ClaimTypes.Name);
        await next(context);
    }
}
