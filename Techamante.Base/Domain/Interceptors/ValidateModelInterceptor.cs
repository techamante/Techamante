using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Techamante.Domain.Interceptors
{
    public class ValidateModelInterceptor:IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();

            }
            catch (Exception ex)
            {
                Console.Write(invocation.Method.Name);
                throw ex;
            }
        }
    }
}
