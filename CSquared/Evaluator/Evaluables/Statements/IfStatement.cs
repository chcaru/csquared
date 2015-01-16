using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class IfStatement : IStatement
    {
        public IExpression Evaluate(CSquaredEnvironment environment)
        {
            var conditionalResult = this.ConditionalExpression.Evaluate(environment);

            var conditional = false;

            if (conditionalResult is CSquaredBoolean)
            {
                conditional = (conditionalResult as CSquaredBoolean).Value;
            }
            else
            {
                conditional = !conditionalResult.IsNull();
            }

            var ifStatementEnvironment = environment.Extend();

            if (conditional)
            {
                foreach (var statement in this.Body)
                {
                    //Add break support?

                    var statementResult = statement.Evaluate(ifStatementEnvironment);

                    if (statement.DoesCarryReturn(statementResult))
                    {
                        return statementResult.CarryReturn();
                    }
                }
            }
            else if (this.ElseContainer != null)
            {
                return this.ElseContainer.Evaluate(environment);
            }

            return new Null();
        }
    }
}