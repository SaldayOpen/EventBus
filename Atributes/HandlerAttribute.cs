using System;

namespace Salday.EventBus
{
    /// <summary>
    /// All handler functions need to be marked with this attribute for reflection lookup
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class HandlerAttribute : Attribute
    {
        public HandlerPriority Priority { get; }

        public HandlerAttribute(HandlerPriority priority)
        {

            this.Priority = priority;
        }
    }
}