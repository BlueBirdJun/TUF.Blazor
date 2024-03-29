﻿using Daniel.Common.Interfaces;
using MediatR;
using TUF.Shared.Notification;

namespace TUF.Front.Client.Infrastructure.Notifications;

 
public class NotificationPublisher : INotificationPublisher
{
    private readonly ILogger<NotificationPublisher> _logger;
    private readonly IPublisher _mediator;

    public NotificationPublisher(ILogger<NotificationPublisher> logger, IPublisher mediator) =>
        (_logger, _mediator) = (logger, mediator);

    public Task PublishAsync(INotificationMessage notification)
    {
        _logger.LogInformation("Publishing Notification : {notification}", notification.GetType().Name);
        return _mediator.Publish(CreateNotificationWrapper(notification));
    }

    private INotification CreateNotificationWrapper(INotificationMessage notification) =>
        (INotification)Activator.CreateInstance(
            typeof(NotificationWrapper<>).MakeGenericType(notification.GetType()), notification)!;
}


public class NotificationWrapper<TNotificationMessage> : INotification
    where TNotificationMessage : INotificationMessage
{
    public NotificationWrapper(TNotificationMessage notification) => Notification = notification;
    public TNotificationMessage Notification { get; }
}