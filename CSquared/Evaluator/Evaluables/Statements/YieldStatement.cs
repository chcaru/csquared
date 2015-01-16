using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class YieldStatement : IStatement
    {
        public IExpression Evaluate(CSquaredEnvironment environment)
        {
            var yieldToIdentifier = this.YieldTo.Value.ToString();

            IExpression expression;

            if (!environment.Lookup(yieldToIdentifier, out expression))
            {
                throw new Exception("Attempted to access undefined variable " + yieldToIdentifier);
            }

            if (!(expression is CSquaredObject))
            {
                throw new Exception(
                    "Attempted to yield to variable " 
                    + yieldToIdentifier
                    + ", a non-yieldable type.");
            }

            var yieldToExpression = expression as CSquaredObject;

            var yieldExpression = this.YieldExpression.Evaluate(environment);

            yieldToExpression.AddToEnd(yieldExpression);

            return new Null();
        }
    }
}
