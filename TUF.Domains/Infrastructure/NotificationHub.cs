using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Finbuckle.MultiTenant;
using TUF.Domains.Common.Exceptions;

namespace TUF.Domains.Infrastructure
{
    public interface ITransientService
    {
    }

    [Authorize]
    public class NotificationHub : Hub, ITransientService
    {
        private readonly ITenantInfo? _currentTenant;
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ITenantInfo? currentTenant, ILogger<NotificationHub> logger)
        {
            _currentTenant = currentTenant;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            if (_currentTenant is null)
            {
                throw new UnauthorizedException("Authentication Failed.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"GroupTenant-{_currentTenant.Id}");

            await base.OnConnectedAsync();

            _logger.LogInformation("A client connected to NotificationHub: {connectionId}", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"GroupTenant-{_currentTenant!.Id}");

            await base.OnDisconnectedAsync(exception);

            _logger.LogInformation("A client disconnected from NotificationHub: {connectionId}", Context.ConnectionId);
        }
    }
}
