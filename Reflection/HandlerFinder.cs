using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Salday.EventBus.Reflection
{
    internal class HandlerFinder
    {
        internal MethodInfo[] GetPublicInstanceMethods(object eventProxy)
        {
            var proxyType = eventProxy.GetType();

            return proxyType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        }

        internal List<HandlerMethodData> GetHandlersData(MethodInfo[] methodInfo)
        {
            HandlerMethodData data;

            var rt = new List<HandlerMethodData>();

            foreach (var method in methodInfo)
            {
                if (TryCreateHandlerData(method, out data)) rt.Add(data);
            }

            return rt;
        }

        internal bool TryCreateHandlerData(MethodInfo methodInfo, out HandlerMethodData data)
        {
            var attribute = methodInfo.GetCustomAttributes(typeof(HandlerAttribute), true).FirstOrDefault() as HandlerAttribute;


            //No attribute found, handler cannot be created
            if (attribute == null)
            {
                data = null;
                return false;
            }

            var methodParams = methodInfo.GetParameters();

            //Method doesn't mach handler pattern, handler cannot be created
            if (methodParams.Length != 1)
            {
                data = null;
                return false;
            }


            var methodParameter = methodParams.FirstOrDefault();

            var paramType = methodParameter.ParameterType;

            var handlerData = new HandlerMethodData()
            {
                AttachedAttribute = attribute,

                Method = methodInfo,

                ParameterType = paramType
            };

            data = handlerData;

            return true;
        }
    }
}
