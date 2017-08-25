using Salday.EventBus.Exceptions;
using Salday.EventBus.Interfaces;
using Salday.EventBus.Reflection;
using System;
using System.Collections.Generic;

namespace Salday.EventBus
{

    public class EventBus : IEventBus
    {
        private readonly bool _throwIfNotRegistered;

        protected IDictionary<Type, IHandlerCollection> TypeToHandler = new Dictionary<Type, IHandlerCollection>();

        protected IDictionary<object, ISubscription> Subscriptions = new Dictionary<object, ISubscription>();

        private readonly HandlerFinder _handlerInfoFinder = new HandlerFinder();

        private readonly HandlerFactory _handlerFactory = new HandlerFactory();

        public EventBus(bool throwIfNotRegistered = false)
        {
            this._throwIfNotRegistered = throwIfNotRegistered;
        }

        public void Publish<TEvent>(TEvent eventObject) where TEvent : EventBase
        {
            var handlerCollection = GetTypedHandlerCollection<TEvent>();

            if (handlerCollection != null)
            {
                handlerCollection.Handle(eventObject);
                return;
            }

            if (_throwIfNotRegistered)
            {
                var type = typeof(TEvent);

                var errorMessage = $"No handler found for published type <{type}>";

                throw new TypeNotRegisteredException(type, errorMessage);
            }
        }

        internal IHandlerCollection<TEvent> GetTypedHandlerCollection<TEvent>() where TEvent : EventBase
        {
            if (TypeToHandler.TryGetValue(typeof(TEvent), out IHandlerCollection coll))
            {
                return coll as IHandlerCollection<TEvent>;
            }

            return null;
        }

        public ISubscription RegisterSubscription(object eventProxy)
        {
            if (Subscriptions.ContainsKey(eventProxy))
            {
                throw new AlreadyRegisteredException(eventProxy, this);
            }

            var methods = _handlerInfoFinder.GetPublicInstanceMethods(eventProxy);

            var handlersMethodData = _handlerInfoFinder.GetHandlersData(methods);

            var handlers = _handlerFactory.CreateHandlers(handlersMethodData, eventProxy);

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
                if (TypeToHandler.TryGetValue(keyValuePair.Key, out IHandlerCollection handlerCollect))
                {
                    handlerCollect.AddHandlers(keyValuePair.Value);
                }
                else
                {
                    var handlCollType = typeof(HandlerCollection<>).MakeGenericType(keyValuePair.Key);
                    var handlCollection = Activator.CreateInstance(handlCollType, this) as IHandlerCollection;
                    TypeToHandler.Add(keyValuePair.Key, handlCollection);
                    handlCollection?.AddHandlers(keyValuePair.Value);
                }

            }

            var sub = new Subscription(eventProxy, this, dict);
            Subscriptions.Add(eventProxy, sub);
            return sub;
        }

        public void RemoveSubscription(ISubscription subscription)
        {
            if (subscription == null) return;

            if (!Subscriptions.ContainsKey(subscription.EventProxy)) return;

            Subscriptions.Remove(subscription.EventProxy);

            var types = subscription.Handlers.Keys;

            foreach (var type in types)
            {
                TypeToHandler[type].RemoveSubscription(subscription);
            }
        }

        public void RemoveType(Type type)
        {
            TypeToHandler.Remove(type);
        }
    }
}
