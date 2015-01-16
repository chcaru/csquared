using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class ForeachStatement : IStatement
    {
        public virtual IExpression Evaluate(CSquaredEnvironment environment)
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

            foreach (var enumeratedExpression in expressionList)
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
                        return statementResult.CarryReturn();
                    }
                }
            }

            return new Null();
        }
    }
}
