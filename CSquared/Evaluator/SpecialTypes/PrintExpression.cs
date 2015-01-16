using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class PrintExpression : InternalExpression
    {
        public PrintExpression() : base("print", 1) { }

        public override IExpression EvaluateBody(CSquaredEnvironment environment)
        {
            var expression = this.GetParameter(environment, 0);

            var expressionResult = expression.Evaluate(environment).CastTo<IPrimary>();

            var resultString = expressionResult.ToString();

            Console.Write(resultString);

            return new Null();
        }
    }
}
