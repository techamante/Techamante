using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using SimpleInjector;
using Techamante.SimpleInjector.Interception;

namespace Techamante.SimpleInjector
{
    public class InterceptableContainer : Container
    {
        public InterceptableContainer()
        {
            var generator = new ProxyGenerator();
            var selector = new InterceptorSelector();
            var options = new ProxyGenerationOptions(new InterceptionHook()) { Selector = selector };

            Func<Type, object, object> createProxy =
             (p, t) => generator.CreateClassProxyWithTarget(p, t, options);

            this.ExpressionBuilt += (s, e) =>
            {
                Console.WriteLine(e.RegisteredServiceType);
               
                var members = e.RegisteredServiceType.FindMembers(MemberTypes.All, BindingFlags.Instance | BindingFlags.Public,
                    new MemberFilter(SearchFilter), "*");

                if (members.Any())
                {
                    e.Expression = Expression.Convert(
                        Expression.Invoke(
                            Expression.Constant(createProxy),
                            Expression.Constant(e.RegisteredServiceType),
                            e.Expression),
                        e.RegisteredServiceType);
                }
            };
        }
        private bool SearchFilter(MemberInfo memObj, object obj)
        {
            var hasAttribute = memObj.HasAttribute<InterceptAttribute>();
            return hasAttribute;
        }

    }
}
