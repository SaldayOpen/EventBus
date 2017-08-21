using System;

namespace Salday.EventBus.Exceptions
{
    /// <summary>
    /// Is thrown, when attempting to register proxy, that is already registered
    /// </summary>
    [Serializable]
    public class AlreadyRegisteredException : Exception
    {
        public IEventBus EventBus { get; }

        public object EventProxy { get; }

        public AlreadyRegisteredException() { }

        public AlreadyRegisteredException(object eventProxy, IEventBus eventBus) : base(GetDefaultMessage(eventProxy, eventBus))
        {
            this.EventProxy = eventProxy;
            this.EventBus = eventBus;
        }

        public AlreadyRegisteredException(string message) : base(message) { }
        public AlreadyRegisteredException(string message, Exception inner) : base(message, inner) { }

        protected static string GetDefaultMessage(object eventProxy, IEventBus eventBus)
        {
            return string.Format("Event proxy: {0} was already registered on this event bus", eventProxy);
        }

        protected AlreadyRegisteredException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
