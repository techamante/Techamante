using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.SimpleInjector.Interception;

namespace Techamante.Domain.Interceptors
{
    public class ValidateModelAttribute:InterceptAttribute
    {



        public override Castle.DynamicProxy.IInterceptor CreateInterceptor()
        {
            return new ValidateModelInterceptor();
        }
    }
}
