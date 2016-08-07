using System;
using Castle.DynamicProxy;
using Techamante.Base.SimpleInjector.Interception;

namespace Techamante.Base.Logging
{
    public class LogAttribute : InterceptAttribute
    {

        public override IInterceptor CreateInterceptor()
        {
            throw new NotImplementedException();
        }
    }
}

    

