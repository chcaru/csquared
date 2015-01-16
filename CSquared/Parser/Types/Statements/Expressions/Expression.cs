using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public static class Expression
    {
        public static bool IsNull(this IExpression expression)
        {
            return expression is Null || expression == null;
        }

        public static ReturnCarrier CarryReturn(this IExpression expression)
        {
            if (expression is ReturnCarrier)
            {
                return expression as ReturnCarrier;
            }

            return new ReturnCarrier
            {
                EvaluatedReturnExpression = expression
            };
        }

        public static bool DoesCarryReturn(this IExpression expression)
        {
            return expression is ReturnCarrier;
        }

        public static bool DoesCarryReturn(this IStatement statement, IExpression statementResult)
        {
            return statement is ReturnStatement || statementResult.DoesCarryReturn();
        }

        public static bool ToInternalBool(this IExpression expression)
        {
            var conditional = false;

            if (expression is CSquaredBoolean)
            {
                conditional = (expression as CSquaredBoolean).Value;
            }
            else
            {
                conditional = !expression.IsNull();
            }

            return conditional;
        }

        public static T CastTo<T>(this IExpression expression)
        {
            if (expression is T)
            {
                return (T)expression;
            }
            
            if (expression is ICastable<T>)
            {
                return (expression as ICastable<T>).CastTo();
            }

            var forcedCast = (T)expression;

            if (forcedCast.IsNull())
            {
                throw new Exception("Attempt to cast " + expression.GetType() + " to " + typeof(T) + " failed.");
            }

            return forcedCast;
        }

        public static bool TryCastTo<T>(this IExpression expression, out T castResult)
        {
            var castable = expression is T || expression is ICastable<T>;

            castResult = castable ? expression.CastTo<T>() : default(T);

            return castable;
        }
    }
}
