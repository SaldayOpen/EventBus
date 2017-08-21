using Salday.EventBus.Interfaces;
using System;


namespace Salday.EventBus
{
    internal sealed class Handler<TEvent> : IHandler<TEvent> where TEvent : EventBase
    {
        /// <summary>
        /// Action that is called when event is received
        /// </summary>
        public Action<TEvent> Action { get; }

        /// <summary>
        /// Type of messages, accepted by this handler
        /// </summary>
        public Type HandledType { get; }

        /// <summary>
        /// Priority of this handler
        /// </summary>
        public HandlerPriority Priority { get; }

        /// <summary>
        /// Subscription, that this handler is bound to
        /// </summary>
        public ISubscription Subscription { get; set; }

        public Handler(Type handledType, HandlerPriority priority, Action<TEvent> action)
        {
            this.HandledType = handledType;
            this.Priority = priority;
            this.Action = action;
        }


        public void Handle(TEvent eventObj)
        {
            if (Subscription.Active)
                Action?.Invoke(eventObj);
        }
    }
}
