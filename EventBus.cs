using Salday.EventBus.Exceptions;
using Salday.EventBus.Interfaces;
using Salday.EventBus.Reflection;
using System;
using System.Collections.Generic;

namespace Salday.EventBus
{

    public class EventBus : IEventBus
    {
        bool throwIfNotRegistered = false;

        protected IDictionary<Type, IHandlerCollection> typeToHandler = new Dictionary<Type, IHandlerCollection>();

        protected IDictionary<object, ISubscription> subscriptions = new Dictionary<object, ISubscription>();

        HandlerFinder handlerInfoFinder = new HandlerFinder();

        HandlerFactory handlerFactory = new HandlerFactory();

        public EventBus(bool throwIfNotRegistered = false)
        {
            this.throwIfNotRegistered = throwIfNotRegistered;
        }

        public void Publish<TEvent>(TEvent eventObject) where TEvent : EventBase
        {
            var handlerCollection = GetTypedHandlerCollection<TEvent>();

            if (handlerCollection != null)
            {
                handlerCollection.Handle(eventObject);
                return;
            }

            if (throwIfNotRegistered)
            {
                var type = typeof(TEvent);

                var errorMessage = string.Format("No handler found for published type <{0}>", type);

                throw new TypeNotRegisteredException(type, errorMessage);
            }
        }

        internal IHandlerCollection<TEvent> GetTypedHandlerCollection<TEvent>() where TEvent : EventBase
        {
            IHandlerCollection coll;

            if (typeToHandler.TryGetValue(typeof(TEvent), out coll))
            {
                return coll as IHandlerCollection<TEvent>;
            }

            else return null;
        }

        public ISubscription RegisterSubscription(object eventProxy)
        {
            if (subscriptions.ContainsKey(eventProxy))
            {
                throw new AlreadyRegisteredException(eventProxy, this);
            }

            var methods = handlerInfoFinder.GetPublicInstanceMethods(eventProxy);

            var handlersMethodData = handlerInfoFinder.GetHandlersData(methods);

            var handlers = handlerFactory.CreateHandlers(handlersMethodData, eventProxy);

            var typeAndHandler = new Dictionary<Type, List<IHandler>>();

            foreach (var handler in handlers)
            {
                if (!typeAndHandler.ContainsKey(handler.HandledType))
                {
                    typeAndHandler.Add(handler.HandledType, new List<IHandler>());
                }

                typeAndHandler[handler.HandledType].Add(handler);
            }

            var dict = new Dictionary<Type, IList<IHandler>>();
            foreach (var keyValuePair in typeAndHandler)
            {
                dict.Add(keyValuePair.Key, keyValuePair.Value);
                IHandlerCollection handlerCollect;
                if (typeToHandler.TryGetValue(keyValuePair.Key, out handlerCollect))
                {
                    handlerCollect.AddHandlers(keyValuePair.Value);
                }
                else
                {
                    var handlCollType = typeof(HandlerCollection<>).MakeGenericType(keyValuePair.Key);
                    var handlCollection = Activator.CreateInstance(handlCollType, this) as IHandlerCollection;
                    typeToHandler.Add(keyValuePair.Key, handlCollection);
                    handlCollection.AddHandlers(keyValuePair.Value);
                }

            }

            var sub = new Subscription(eventProxy, this, dict);
            subscriptions.Add(eventProxy, sub);
            return sub;
        }

        public void RemoveSubscription(ISubscription subscription)
        {
            if (subscription == null) return;

            if (!subscriptions.ContainsKey(subscription.EventProxy)) return;

            subscriptions.Remove(subscription.EventProxy);

            var types = subscription.Handlers.Keys;

            foreach (var type in types)
            {
                typeToHandler[type].RemoveSubscription(subscription);
            }
        }

        public void RemoveType(Type type)
        {
            typeToHandler.Remove(type);
        }
    }
}
