using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Extensions
{
    public static class EntityLinq
    {
        public static TOut Cast<TOut,TIn>(this TIn entity,Expression<Func<TIn,TOut>> expression)
            where TIn : class
            where TOut  : class
        {
            // Create a parameter for the expression
            ParameterExpression parameter = Expression.Parameter(typeof(TIn), "entity");

            // Create the expression tree for the cast
            Expression<TOut> castExpression = Expression.Lambda<TOut>(expression.Body, parameter);

            // Invoke the cast expression
            TOut result = castExpression.Compile();

            // Return the result
            return result;
        }
    }
}
