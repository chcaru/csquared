using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class CastExpression<T> : InternalExpression where T : IPrimary
    {
        public CastExpression() : base(string.Empty, 1) { }

        public override IExpression EvaluateBody(CSquaredEnvironment environment)
        {
            var expression = this.GetParameter(environment, 0);

            try
            {
                return expression.Evaluate(environment).CastTo<T>();
            }
            catch
            {
                return new Null();
            }
        }
    }
}