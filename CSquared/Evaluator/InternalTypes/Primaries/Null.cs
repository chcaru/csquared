using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class Null : Primary<object, Null>, ICastable<CSquaredBoolean>
    {
        public override object Value { get { return null; } }

        public override IExpression Initialize(CSquaredEnvironment environment)
        {
            return new Null();
        }

        CSquaredBoolean ICastable<CSquaredBoolean>.CastTo()
        {
            return CSquaredBoolean.False;
        }

        public override CSquaredBoolean Equal(CSquaredEnvironment environment, IExpression expression)
        {
            return CSquaredBoolean.From(expression is Null);
        }

        public override string ToString()
        {
            return "null";
        }
    }
}
