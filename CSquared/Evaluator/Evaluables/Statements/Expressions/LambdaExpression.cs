using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class LambdaExpression : Primary<IExpression, LambdaExpression>
    {
        public CSquaredEnvironment Closure { get; set; }

        public override IExpression Initialize(CSquaredEnvironment environment)
        {
            this.Closure = environment;

            return this;
        }
        
        public virtual IExpression EvaluateBody(CSquaredEnvironment environment)
        {
            foreach (var statement in this.Body)
            {
                var statementResult = statement.Evaluate(environment);

                if (statement.DoesCarryReturn(statementResult))
                {
                    this.Value = statementResult;

                    return statementResult;
                }
            }

            return new Null();
        }

        public override string ToString()
        {
            return "Lambda (" + this.GetHashCode() + ")";
        }
    }
}
