using Daniel.Common.Interfaces;

namespace TUF.Front.Client.Infrastructure.Notifications;

public record ConnectionStateChanged(ConnectionState State, string? Message) : INotificationMessage;
