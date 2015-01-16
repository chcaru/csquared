using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class Integer : Primary<int, Integer>, ICastable<Real>
    {
        Real ICastable<Real>.CastTo()
        {
            return new Real
            {
                Value = this.Value
            };
        }

        public override IPrimary Add(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment).CastTo<Integer>();

            return Integer.From(this.Value + evaluatedExpression.Value);
        }

        public override IPrimary Subtract(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment).CastTo<Integer>();

            return Integer.From(this.Value - evaluatedExpression.Value);
        }

        public override IPrimary Divide(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment).CastTo<Integer>();

            return Integer.From(this.Value / evaluatedExpression.Value);
        }

        public override IPrimary Multiply(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment).CastTo<Integer>();

            return Integer.From(this.Value * evaluatedExpression.Value);
        }

        public override CSquaredBoolean GreaterThan(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment).CastTo<Integer>();

            return CSquaredBoolean.From(this.Value > evaluatedExpression.Value);
        }

        public override CSquaredBoolean LessThan(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment).CastTo<Integer>();

            return CSquaredBoolean.From(this.Value < evaluatedExpression.Value);
        }

        public override CSquaredBoolean Equal(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment).CastTo<Integer>();

            return CSquaredBoolean.From(this.Value == evaluatedExpression.Value);
        }
    }
}
