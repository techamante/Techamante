using System;
using Castle.DynamicProxy;

namespace Techamante.Base.Logging
{
    public class LogInterceptor : IInterceptor
    {

        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Started " + invocation.GetConcreteMethod().Name);
            invocation.Proceed();
            Console.WriteLine("Ended");
        }
    }
}
