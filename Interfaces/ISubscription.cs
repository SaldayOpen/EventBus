using Salday.EventBus.Interfaces;
using System;
using System.Collections.Generic;

namespace Salday.EventBus
{
    public interface ISubscription : IDisposable
    {
        /// <summary>
        /// Object used for handler lookup
        /// </summary>
        object EventProxy { get; }

        /// <summary>
        /// Event bus, that this subscribtion belong to
        /// </summary>
        IEventBus EventBus { get; }

        /// <summary>
        /// Defines, if handlers are active (It is not restrictive, should check that inside handler manually)
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// All registred handlers
        /// </summary>
        IDictionary<Type, IList<IHandler>> Handlers { get; }
    }
}