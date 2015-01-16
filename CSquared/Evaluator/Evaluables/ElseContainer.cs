using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class ElseContainer
    {
        public IExpression Evaluate(CSquaredEnvironment environment)
        {
            if (this.IfStatement != null)
            {
                return this.IfStatement.Evaluate(environment);
            }
            else if (this.Body != null)
            {
                var elseBodyEnvironment = environment.Extend();

                foreach (var statement in this.Body)
                {
                    var statementResult = statement.Evaluate(elseBodyEnvironment);

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