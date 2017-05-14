using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Patterns
{
    public static class SpecificationExtensions
    {
        public static ISpecification<T> And<T>(this ISpecification<T> spec1, ISpecification<T> spec2)
        {
            return new AndSpecification<T>(spec1, spec2);
        }

        public static ISpecification<T> And<T>(this ExpressionSpecification<T> spec1, ExpressionSpecification<T> spec2)
        {
            return new AndExpressionSpecification<T>(spec1, spec2);
        }

        public static ISpecification<T> Or<T>(this ExpressionSpecification<T> spec1, ExpressionSpecification<T> spec2)
        {
            return new OrExpressionSpecification<T>(spec1, spec2);
        }
    }
}
