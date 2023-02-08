using Daniel.Common.Interfaces;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Infrastructure.Common.Interfaces;
using TUF.Shared.Notification;


namespace TUF.Infrastructure.Notifications;

public class NotificationSender //: INotificationSender
{
    private readonly IHubContext<NotificationHub> _notificationHubContext;

    public NotificationSender(IHubContext<NotificationHub> notificationHubContext) =>
        (_notificationHubContext) = (notificationHubContext);

    public Task BroadcastAsync(INotificationMessage notification, CancellationToken cancellationToken) =>
        _notificationHubContext.Clients.All
            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
     
    
    public Task BroadcastAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
        _notificationHubContext.Clients.AllExcept(excludedConnectionIds)
            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

    public Task SendToAllAsync(INotificationMessage notification, CancellationToken cancellationToken) =>
        _notificationHubContext.Clients.Group($"GroupTenant-")
            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

    public Task SendToAllAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
        _notificationHubContext.Clients.GroupExcept($"GroupTenant- ", excludedConnectionIds)
            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

    public Task SendToGroupAsync(INotificationMessage notification, string group, CancellationToken cancellationToken) =>
        _notificationHubContext.Clients.Group(group)
            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

    public Task SendToGroupAsync(INotificationMessage notification, string group, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
        _notificationHubContext.Clients.GroupExcept(group, excludedConnectionIds)
            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

    public Task SendToGroupsAsync(INotificationMessage notification, IEnumerable<string> groupNames, CancellationToken cancellationToken) =>
        _notificationHubContext.Clients.Groups(groupNames)
            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

    public Task SendToUserAsync(INotificationMessage notification, string userId, CancellationToken cancellationToken) =>
        _notificationHubContext.Clients.User(userId)
            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

    public Task SendToUsersAsync(INotificationMessage notification, IEnumerable<string> userIds, CancellationToken cancellationToken) =>
        _notificationHubContext.Clients.Users(userIds)
            .SendAsync(NotificationConstants.NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
}
