using Salday.EventBus.Interfaces;
using System;
using System.Collections.Generic;

namespace Salday.EventBus
{
    internal class Subscription : ISubscription
    {
        /// <summary>
        /// Object used for handler lookup
        /// </summary>
        public object EventProxy { get; }

        /// <summary>
        /// Event bus, that this subscribtion belong to
        /// </summary>
        public IEventBus EventBus { get; }

        /// <summary>
        /// Defines, if handlers are active (It is not restrictive, should check that inside handler manually)
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// All registred handlers
        /// </summary>
        public IDictionary<Type, IList<IHandler>> Handlers { get; }


        public Subscription(object eventProxy, IEventBus evBus, IDictionary<Type, IList<IHandler>> handlers)
        {
            this.EventProxy = eventProxy;
            this.EventBus = evBus;
            this.Handlers = handlers;

            foreach (var handlerCollection in handlers.Values)
            {
                foreach (var handler in handlerCollection)
                {
                    handler.Subscription = this;
                }
            }
        }

        /// <summary>
        /// Removes subsription from event bus
        /// </summary>
        public void Dispose()
        {
            EventBus.RemoveSubscription(this);
        }
    }
}
