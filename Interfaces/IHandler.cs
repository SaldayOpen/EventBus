using System;

namespace Salday.EventBus.Interfaces
{
    public interface IHandler
    {
        Type HandledType { get; }

        ISubscription Subscription { get; set; }

        HandlerPriority Priority { get; }
    }
}
