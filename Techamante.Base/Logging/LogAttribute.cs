using System;
using Castle.DynamicProxy;
using Techamante.SimpleInjector.Interception;

namespace Techamante.Logging
{
    public class LogAttribute : InterceptAttribute
    {

        public override IInterceptor CreateInterceptor()
        {
            throw new NotImplementedException();
        }
    }
}

    

