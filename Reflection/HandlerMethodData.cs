using System;
using System.Reflection;

namespace Salday.EventBus.Reflection
{
    internal class HandlerMethodData
    {
        public MethodInfo Method { get; set; }

        public HandlerAttribute AttachedAttribute { get; set; }

        public Type ParameterType { get; set; }

    }
}
