using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class CSquaredString : 
        Primary<string, CSquaredString>, 
        IEnumerable<IExpression>,
        ICastable<Integer>, 
        ICastable<Real>, 
        ICastable<CSquaredBoolean>
    {
        private IEnumerable<IExpression> Enumerator
        {
            get { return this.Value.Select(c => CSquaredString.From(c.ToString())); }
        }

        Integer ICastable<Integer>.CastTo()
        {
            double real;

            if (!double.TryParse(this.Value, out real))
            {
                real = this.Value.Sum(c => c);
            }

            return Integer.From((int)real);
        }

        Real ICastable<Real>.CastTo()
        {
            double real;

            if (!double.TryParse(this.Value, out real))
            {
                throw new Exception("Attempted to cast a string to a real which was incorrect to suit the conversion: " + this.Value);
            }

            return Real.From(real);
        }

        CSquaredBoolean ICastable<CSquaredBoolean>.CastTo()
        {
            bool boolean;

            if (!bool.TryParse(this.Value, out boolean))
            {
                boolean = this.Value == string.Empty;
            }

            return CSquaredBoolean.From(boolean);
        }

        IEnumerator<IExpression> IEnumerable<IExpression>.GetEnumerator()
        {
            return this.Enumerator.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Enumerator.GetEnumerator();
        }

        public override IPrimary Add(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment);

            return CSquaredString.From(this.Value + evaluatedExpression.ToString());
        }

        public override CSquaredBoolean Equal(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedExpression = expression.Evaluate(environment).ToString();

            return CSquaredBoolean.From(this.Value == evaluatedExpression);
        }
    }
}