using System;
using Castle.Core.Internal;
using Castle.DynamicProxy;

namespace Techamante.Base.SimpleInjector.Interception
{
    public class InterceptionHook:IProxyGenerationHook
    {
        public void MethodsInspected()
        {
            
        }

        public void NonProxyableMemberNotification(Type type, System.Reflection.MemberInfo memberInfo)
        {
            //throw new NotImplementedException();
        }

        public bool ShouldInterceptMethod(Type type, System.Reflection.MethodInfo methodInfo)
        {
            return methodInfo.HasAttribute<InterceptAttribute>();
        }
    }
}
