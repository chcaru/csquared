using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class CountExpression : InternalExpression
    {
        public CountExpression() : base("count", 1) { }

        public override IExpression EvaluateBody(CSquaredEnvironment environment)
        {
            var expression = this.GetParameter(environment, 0);

            var enumerableExpression = expression.CastTo<IEnumerable<IExpression>>();

            if (enumerableExpression.IsNull())
            {
                throw new Exception("Attempted to call empty on a non-enumerable type");
            }

            return Integer.From(enumerableExpression.Count());
        }
    }
}
