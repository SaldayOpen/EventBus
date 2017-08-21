using Salday.EventBus.Interfaces;
using System.Collections.Generic;

namespace Salday.EventBus.Interfaces
{
    public interface IHandlerCollection
    {
        IEventBus EvBus { get; }

        void AddHandlers(IList<IHandler> list);
        void RemoveSubscription(ISubscription supscription);
    }
}
