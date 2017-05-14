using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Patterns
{
    public abstract class ExpressionSpecification<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();


        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = ToExpression().Compile();
            return predicate(entity);
        }

    }
}
