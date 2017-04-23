using System;
using Castle.DynamicProxy;

namespace Techamante.SimpleInjector.Interception
{
    public abstract class InterceptAttribute:Attribute
    {
        public abstract IInterceptor CreateInterceptor();

    }
}
