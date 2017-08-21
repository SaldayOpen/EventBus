using Salday.EventBus;
using Salday.EventBus.Interfaces;
using Salday.EventBus.Reflection;
using System;
using System.Collections.Generic;

namespace Salday.EventBus.Reflection
{
    internal class HandlerFactory
    {
        internal List<IHandler> CreateHandlers(IList<HandlerMethodData> handlersData, object eventProxy)
        {
            var handlers = new List<IHandler>();

            foreach (var handlerData in handlersData)
            {
                handlers.Add(CreateHandler(handlerData, eventProxy));
            }

            return handlers;
        }

        internal IHandler CreateHandler(HandlerMethodData handlerData, object eventProxy)
        {
            var paramType = handlerData.ParameterType;


            //Creates Handler<TEvent> type using TEvent from method parameter type
            var handlerType = typeof(Handler<>).MakeGenericType(paramType);


            //Creates Action<T> type using T from method parameter type
            var actionType = typeof(Action<>).MakeGenericType(paramType);


            //Creates instance of Action<T> using T from method parameter type
            var action = Delegate.CreateDelegate(actionType, eventProxy, handlerData.Method);


            //Creates instance of Handler<TEvent> using TEvent param type , handler priority from attribute and action from Methodinfo
            var handler = Activator.CreateInstance(handlerType, paramType, handlerData.AttachedAttribute.Priority, action);

            return handler as IHandler;

        }
    }
}
