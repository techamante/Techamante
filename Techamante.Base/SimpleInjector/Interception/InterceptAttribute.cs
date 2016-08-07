using System;
using Castle.DynamicProxy;

namespace Techamante.Base.SimpleInjector.Interception
{
    public abstract class InterceptAttribute:Attribute
    {
        public abstract IInterceptor CreateInterceptor();

    }
}
