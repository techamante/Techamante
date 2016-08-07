using System;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace Techamante.Base.SimpleInjector.Interception
{
    public class InterceptorSelector:IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, System.Reflection.MethodInfo method, IInterceptor[] interceptors)
        {
            var interceptorList = new List<IInterceptor>();

            var interceptAttributes = method.GetCustomAttributes(typeof (InterceptAttribute), true);

            interceptAttributes.ForEach(attr =>
            {
                var attribute = (InterceptAttribute) attr;
                interceptorList.Add(attribute.CreateInterceptor());

            });

            return interceptorList.ToArray();

        }
    }
}
