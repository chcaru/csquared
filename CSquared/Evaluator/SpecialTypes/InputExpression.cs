using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class InputExpression : InternalExpression
    {
        public InputExpression() : base("input") { }

        public override IExpression EvaluateBody(CSquaredEnvironment environment)
        {
            var input = Console.ReadLine();

            bool boolean;

            if (bool.TryParse(input, out boolean))
            {
                return CSquaredBoolean.From(boolean);
            }

            int integer;

            if (int.TryParse(input, out integer))
            {
                return Integer.From(integer);
            }

            double real;

            if (double.TryParse(input, out real))
            {
                return Real.From(real);
            }

            return CSquaredString.From(input);
        }
    }
}
