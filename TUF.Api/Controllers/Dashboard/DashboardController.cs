//using NSwag.Annotations;
using NSwag.Annotations;
using TUF.Application.Dashboard;
using TUF.Shared.Authorization;
using TUF.Shared.Dtos.Dashboard;

namespace TUF.Api.Controllers.Dashboard;

public class DashboardController : VersionedApiController
{
    [HttpGet]
    [MustHavePermission(TUFAction.View, TUFResource.Dashboard)]
    [OpenApiOperation("Get statistics for the dashboard.", "")]
    public Task<StatsDto> GetAsync()
    {
         
        //var cc= TufAct2.Search;

        return Mediator.Send(new GetStatsRequest());
    }
}
