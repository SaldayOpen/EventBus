using Salday.EventBus.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Salday.EventBus
{
    internal class HandlerCollection<TEvent> : IHandlerCollection<TEvent> where TEvent : EventBase
    {
        public IEventBus EvBus { get; }

        IList<IHandler<TEvent>> _handlers = new List<IHandler<TEvent>>();


        public HandlerCollection(IEventBus evBus)
        {
            this.EvBus = evBus;
        }

        public HandlerCollection(IEventBus evBus, IList<IHandler<TEvent>> handlers) : this(evBus)
        {
            this._handlers = handlers;
        }

        public void Handle(TEvent eventObject)
        {
            foreach (var t in _handlers)
            {
                t?.Handle(eventObject);
            }
        }

        public void RemoveSubscription(ISubscription supscription)
        {
            var handlersToRemove = supscription.Handlers[typeof(TEvent)];

            //Copy handlers over, to prevent handler collection change upon removal
            var modifiedHandlerCollection = new List<IHandler<TEvent>>(_handlers);

            for (var i = 0; i < modifiedHandlerCollection.Count; i++)
            {
                foreach (var t in handlersToRemove)
                {
                    if (t as IHandler<TEvent> == _handlers[i])
                    {
                        modifiedHandlerCollection.RemoveAt(i);
                    }
                }
            }
            if (_handlers.Count == 0) EvBus.RemoveType(typeof(TEvent));
            else
                _handlers = modifiedHandlerCollection;
        }

        public void AddHandlers(IList<IHandler> list)
        {
            var casted = list.Cast<IHandler<TEvent>>().ToList();

            foreach (var handler in casted)
            {
                _handlers.Add(handler);
            }

            _handlers = _handlers.OrderByDescending(h => h.Priority).ToList();
        }
    }
}
