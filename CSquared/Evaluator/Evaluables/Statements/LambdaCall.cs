using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class LambdaCall : IStatement
    {
        public IExpression Evaluate(CSquaredEnvironment environment)
        {
            var identifier = this.Identifier.Value.ToString();

            IExpression expression;

            if (!environment.Lookup(identifier, out expression))
            {
                throw new Exception("Attempted to access undeclared variable: " + identifier);
            }

            var lambdaExpression = expression.CastTo<LambdaExpression>();

            if (lambdaExpression.IsNull())
            {
                throw new Exception("Attempted to call non-lambda expression: " + identifier + ", as lambda expression");
            }

            var lambdaEnvironment = lambdaExpression.Closure ?? environment;

            lambdaEnvironment = lambdaEnvironment.Extend();

            foreach (var argParam in lambdaExpression.Parameters.Pair(this.Arguments))
            {
                var parameterName = argParam.Item1.Value.ToString();

                var evaluatedArg = argParam.Item2.Evaluate(environment);

                lambdaEnvironment.Set(parameterName, evaluatedArg);
            }

            var evaluatedLambdaExpression = lambdaExpression.EvaluateBody(lambdaEnvironment);

            return
                lambdaExpression.DoesCarryReturn(evaluatedLambdaExpression) ?
                    evaluatedLambdaExpression.Evaluate(lambdaEnvironment) :
                    evaluatedLambdaExpression;
        }
    }
}