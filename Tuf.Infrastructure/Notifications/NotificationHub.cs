﻿using Daniel.Common.Interfaces;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Domains.Common.Exceptions;

namespace TUF.Infrastructure.Notifications;

[Authorize]
public class NotificationHub : Hub, ITransient
{
    private static readonly Dictionary<string, string> userLookup = new Dictionary<string, string>();

    private readonly ILogger<NotificationHub> _logger;
    public NotificationHub( ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        
        await base.OnConnectedAsync();
        _logger.LogInformation("A client connected to NotificationHub: {connectionId}", Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {   await base.OnDisconnectedAsync(exception);
        _logger.LogInformation("A client disconnected from NotificationHub: {connectionId}", Context.ConnectionId);
    }
}