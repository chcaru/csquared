using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class CSquaredBoolean : Primary<bool, CSquaredBoolean>
    {
        public static CSquaredBoolean False
        {
            get { return new CSquaredBoolean { Value = false }; }
        }

        public static CSquaredBoolean True
        {
            get { return new CSquaredBoolean { Value = true }; }
        }

        public override CSquaredBoolean And(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment).CastTo<CSquaredBoolean>();

            return CSquaredBoolean.From(this.Value && evaluatedExpression.Value);
        }

        public override CSquaredBoolean Or(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment).CastTo<CSquaredBoolean>();

            return CSquaredBoolean.From(this.Value || evaluatedExpression.Value);
        }

        public override CSquaredBoolean Equal(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment).CastTo<CSquaredBoolean>();

            return CSquaredBoolean.From(this.Value == evaluatedExpression.Value);
        }

        public override CSquaredBoolean Not(CSquaredEnvironment environment)
        {
            return CSquaredBoolean.From(!this.Value);
        }
    }
}