using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class TypeCheckExpression<T> : InternalExpression where T : IPrimary
    {
        public TypeCheckExpression() : base(string.Empty, 1) { }

        public override IExpression EvaluateBody(CSquaredEnvironment environment)
        {
            var expression = this.GetParameter(environment, 0);

            var expressionResult = expression.Evaluate(environment);

            return CSquaredBoolean.From(expressionResult is T);
        }
    }
}