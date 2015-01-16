using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class EmptyExpression : InternalExpression
    {
        public EmptyExpression() : base("empty", 1) { }

        public override IExpression EvaluateBody(CSquaredEnvironment environment)
        {
            var expression = this.GetParameter(environment, 0);

            var enumerableExpression = expression.CastTo<IEnumerable<IExpression>>();

            if (enumerableExpression.IsNull())
            {
                throw new Exception("Attempted to call empty on a non-enumerable type");
            }

            return CSquaredBoolean.From(enumerableExpression.IsEmpty());
        }
    }
}
