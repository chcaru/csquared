using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class LockStatement : IStatement
    {
        public IExpression Evaluate(CSquaredEnvironment environment)
        {
            var expressionIdentifier = this.Identifier.Value.ToString();

            CSquaredEnvironment.EnvironmentConstituent environmentConsituent;

            if (!environment.BoxedLookup(expressionIdentifier, out environmentConsituent))
            {
                throw new Exception("Attempted to lock undefined variable " + expressionIdentifier);
            }

            lock (environmentConsituent)
            {
                foreach (var statement in this.Body)
                {
                    var statementResult = statement.Evaluate(environment);

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
