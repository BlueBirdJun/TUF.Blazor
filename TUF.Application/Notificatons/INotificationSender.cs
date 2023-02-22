using Daniel.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Application.Notificatons;

public interface   INotificationSender //:ITransient
{
    Task SendToAllAsync(INotificationMessage notification, CancellationToken cancellationToken);
    Task SendToGroupAsync(INotificationMessage notification, string group, CancellationToken cancellationToken);
    Task SendToUserAsync(INotificationMessage notification, string userId, CancellationToken cancellationToken);
}
