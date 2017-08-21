using System;


namespace Salday.EventBus.Interfaces
{
    public interface IHandler<TEvent> : IHandler where TEvent : EventBase
    {
        /// <summary>
        /// Handler action
        /// </summary>
        Action<TEvent> Action { get; }

        /// <summary>
        /// Called when apropriate messega is being dispatched
        /// </summary>
        /// <param name="eventObj"></param>
        void Handle(TEvent eventObj);
    }
}
