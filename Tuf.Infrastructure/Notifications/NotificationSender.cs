using Daniel.Common.Interfaces;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Application.Notificatons;
using TUF.Domains.Entities;
using TUF.Infrastructure.Common.Interfaces;
using TUF.Shared.Notification;


namespace TUF.Infrastructure.Notifications;
public static class NotificationConstants
{
    // The name of the method that's used to send notification messages from the server to the client
    public const string NotificationFromServer = nameof(NotificationFromServer);
}

public class NotiSender : INotificationSender
{
    private readonly IHubContext<NotificationsHub2> _notificationHubContext;
    private readonly ITenantInfo _currentTenant;
    public NotiSender(IHubContext<NotificationsHub2> notificationHubContext, ITenantInfo currentTenant) =>
        (_notificationHubContext, _currentTenant) = (notificationHubContext, currentTenant);

    public async Task SendToAllAsync(INotificationMessage notification, CancellationToken cancellationToken)
    {        
        await _notificationHubContext.Clients.Group($"GroupTenant-_currentTenant.Id")
            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
    }

    public Task SendToGroupAsync(INotificationMessage notification, string group, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SendToUserAsync(INotificationMessage notification, string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
//public class NotificationSender : INotificationSender
//{
//    private readonly IHubContext<NotificationHub> _notificationHubContext;

//    public NotificationSender(IHubContext<NotificationHub> notificationHubContext) =>
//        (_notificationHubContext) = (notificationHubContext);

//    public Task BroadcastAsync(INotificationMessage notification, CancellationToken cancellationToken) =>
//        _notificationHubContext.Clients.All
//            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);


//    public Task BroadcastAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
//        _notificationHubContext.Clients.AllExcept(excludedConnectionIds)
//            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

//    public Task SendToAllAsync(INotificationMessage notification, CancellationToken cancellationToken) =>
//        _notificationHubContext.Clients.Group($"GroupTenant-")
//            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

//    public Task SendToAllAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
//        _notificationHubContext.Clients.GroupExcept($"GroupTenant- ", excludedConnectionIds)
//            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

//    public Task SendToGroupAsync(INotificationMessage notification, string group, CancellationToken cancellationToken) =>
//        _notificationHubContext.Clients.Group(group)
//            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

//    public Task SendToGroupAsync(INotificationMessage notification, string group, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
//        _notificationHubContext.Clients.GroupExcept(group, excludedConnectionIds)
//            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

//    public Task SendToGroupsAsync(INotificationMessage notification, IEnumerable<string> groupNames, CancellationToken cancellationToken) =>
//        _notificationHubContext.Clients.Groups(groupNames)
//            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

//    public Task SendToUserAsync(INotificationMessage notification, string userId, CancellationToken cancellationToken) =>
//        _notificationHubContext.Clients.User(userId)
//            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

//    public Task SendToUsersAsync(INotificationMessage notification, IEnumerable<string> userIds, CancellationToken cancellationToken) =>
//        _notificationHubContext.Clients.Users(userIds)
//            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
//}
