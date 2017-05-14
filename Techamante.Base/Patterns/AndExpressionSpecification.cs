using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Patterns
{
    public class AndExpressionSpecification<T> : ExpressionSpecification<T>
    {
        private readonly ExpressionSpecification<T> _left;
        private readonly ExpressionSpecification<T> _right;


        public AndExpressionSpecification(ExpressionSpecification<T> left, ExpressionSpecification<T> right)
        {
            _right = right;
            _left = left;
        }


        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            Expression<Func<T, bool>> rightExpression = _right.ToExpression();

            BinaryExpression andExpression = Expression.AndAlso(leftExpression.Body, rightExpression.Body);

            return Expression.Lambda<Func<T, bool>>(andExpression, leftExpression.Parameters.Single());
        }
    }
}
