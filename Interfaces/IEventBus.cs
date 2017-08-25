using System;

namespace Salday.EventBus
{
    public interface IEventBus
    {
        /// <summary>
        /// Registerers an object to event bus
        /// </summary>
        /// <param name="eventProxy">Object, which contains handlers marked with Salday.EventBus.HandlerAttribute />
        /// </param>
        /// <returns></returns>
        ISubscription RegisterSubscription(object eventProxy);

        /// <summary>
        /// Publish event data to event bus
        /// </summary>
        /// <typeparam name="TEvent">Type of event occured</typeparam>
        /// <param name="eventObject">Event data</param>
        void Publish<TEvent>(TEvent eventObject) where TEvent : EventBase;

        /// <summary>
        /// Deletes a subscription from event bus
        /// </summary>
        /// <param name="supscription"></param>
        void RemoveSubscription(ISubscription supscription);

        /// <summary>
        /// Removes all handlers, that accept particular event type
        /// </summary>
        /// <param name="type"></param>
        void RemoveType(Type type);
    }
}