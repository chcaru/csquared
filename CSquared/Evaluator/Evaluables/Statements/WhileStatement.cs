using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class WhileStatement : IStatement
    {
        public IExpression Evaluate(CSquaredEnvironment environment)
        {
            var conditionalResult = this.ConditionalExpression.Evaluate(environment);

            var conditional = conditionalResult.ToInternalBool();

            var whileBodyEnvironment = environment.Extend();

            while (conditional)
            {
                foreach (var statement in this.Body)
                {
                    var statementResult = statement.Evaluate(whileBodyEnvironment);

                    if (statement.DoesCarryReturn(statementResult))
                    {
                        return statementResult.CarryReturn();
                    }
                }

                conditionalResult = this.ConditionalExpression.Evaluate(environment);

                conditional = conditionalResult.ToInternalBool();
            }

            return new Null();
        }
    }
}
