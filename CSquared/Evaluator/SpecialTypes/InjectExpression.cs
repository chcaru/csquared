using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class InjectExpression : InternalExpression
    {
        public InjectExpression() : base("inject", 1) { }

        public override IExpression EvaluateBody(CSquaredEnvironment environment)
        {
            var expression = this.GetParameter(environment, 0);

            var expressionResult = expression.Evaluate(environment).CastTo<IPrimary>();

            var resultString = expressionResult.ToString();

            var reader = new Reader();

            reader.SetFileContents(resultString);

            try
            {
                var lexer = new Lexer(reader);

                var parser = new Parser(lexer);

                var evaluator = new Evaluator(environment);

                var program = parser.Parse();

                evaluator.Evaluate(program);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new Null();
        }
    }
}
