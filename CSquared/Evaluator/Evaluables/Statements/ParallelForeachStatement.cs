using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class ParallelForeachStatement : ForeachStatement, IStatement
    {
        public override IExpression Evaluate(CSquaredEnvironment environment)
        {
            var inExpressionIdentifier = this.InIdentifier.Value.ToString();

            IExpression expression;

            if (!environment.Lookup(inExpressionIdentifier, out expression))
            {
                throw new Exception("Attempted to access undefined variable " + inExpressionIdentifier);
            }

            if (!(expression is IEnumerable<IExpression>))
            {
                throw new Exception(
                    "Attempted to enumerate a non-enumerable. "
                    + inExpressionIdentifier
                    + " must be a list or object.");
            }

            var expressionList = expression as IEnumerable<IExpression>;

            expressionList.AsParallel().ForAll(
                (enumeratedExpression) =>
                {
                    var foreachBodyEnvironment = environment.Extend();

                    var enumerationIdentifier = this.OutIdentifier.Value.ToString();

                    var evaluatedEnumeration = enumeratedExpression.Evaluate(environment);

                    foreachBodyEnvironment.Set(enumerationIdentifier, evaluatedEnumeration);

                    foreach (var statement in this.Body)
                    {
                        var statementResult = statement.Evaluate(foreachBodyEnvironment);

                        if (statement.DoesCarryReturn(statementResult))
                        {
                            throw new Exception("Invalid return statement inside parallel foreach.");
                        }
                    }
                });

            return new Null();
        }
    }
}
