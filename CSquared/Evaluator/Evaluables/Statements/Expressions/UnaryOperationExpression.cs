using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class UnaryOperationExpression : IExpression
    {
        public IExpression Evaluate(CSquaredEnvironment environment)
        {
            var expressionAsPrimary = this.Expression.Evaluate(environment).CastTo<IPrimary>();

            if (expressionAsPrimary == null)
            {
                throw new Exception("Attempted to perform a primary operation on a non-primary.");
            }

            switch (this.UnaryOperator.Type)
            {
                case LexemeType.Not:
                    return expressionAsPrimary.CastTo<CSquaredBoolean>().Not(environment);
            }

            return new Null();
        }
    }
}