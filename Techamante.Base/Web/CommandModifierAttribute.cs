using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Techamante.SimpleInjector.Interception;

namespace Techamante.Web
{
    public class CommandModifierAttribute : InterceptAttribute
    {
        public override IInterceptor CreateInterceptor()
        {
            return new CommandModifierInterceptor();
        }
    }
}
