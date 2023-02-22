using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Infrastructure.Notifications;

public class NotificationsHub2 :Hub
{
    public NotificationsHub2() { }

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Group1");
        await base.OnConnectedAsync();        
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    { 
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Group1");
        await base.OnDisconnectedAsync(exception);
    }
}
