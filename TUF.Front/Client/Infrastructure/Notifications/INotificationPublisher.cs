using Daniel.Common.Interfaces;

namespace TUF.Front.Client.Infrastructure.Notifications;

public interface INotificationPublisher
{
    Task PublishAsync(INotificationMessage notification);
}