using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testcon;

public class EventProducer
{
    public event Action ExmaleEvent;
}

public class ExampleEvent : INotification
{
    public ExampleEvent(int notificationCount) => NotificationCount = notificationCount;

    public int NotificationCount { get; }
    //Your implementation own implementation properties, methods, etc...
}