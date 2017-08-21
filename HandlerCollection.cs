using Salday.EventBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Salday.EventBus
{
    internal class HandlerCollection<TEvent> : IHandlerCollection<TEvent> where TEvent : EventBase
    {
        public IEventBus EvBus { get; }

        IList<IHandler<TEvent>> handlers = new List<IHandler<TEvent>>();


        public HandlerCollection(IEventBus evBus)
        {
            this.EvBus = evBus;
        }

        public HandlerCollection(IEventBus evBus, IList<IHandler<TEvent>> handlers) : this(evBus)
        {
            this.handlers = handlers;
        }

        public void Handle(TEvent eventObject)
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.Handle(eventObject);
            }
        }

        public void RemoveSubscription(ISubscription supscription)
        {
            var handlersToRemove = supscription.Handlers[typeof(TEvent)];

            //Copy handlers over, to prevent handler collection change upon removal
            var modifiedHandlerCollection = new List<IHandler<TEvent>>(handlers);

            for (int i = 0; i < modifiedHandlerCollection.Count; i++)
            {
                for (int j = 0; j < handlersToRemove.Count; j++)
                {
                    if (handlersToRemove[j] as IHandler<TEvent> == handlers[i])
                    {
                        modifiedHandlerCollection.RemoveAt(i);
                    }
                }
            }
            if (handlers.Count == 0) EvBus.RemoveType(typeof(TEvent));
            else
                handlers = modifiedHandlerCollection;
        }

        public void AddHandlers(IList<IHandler> list)
        {
            var casted = list.Cast<IHandler<TEvent>>().ToList();

            foreach (var handler in casted)
            {
                handlers.Add(handler);
            }

            handlers = handlers.OrderByDescending(h => h.Priority).ToList();
        }
    }
}
