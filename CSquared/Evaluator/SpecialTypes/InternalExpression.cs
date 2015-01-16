using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public abstract class InternalExpression : LambdaExpression
    {
        public string[] ParameterNames { get; private set; }
        public int ParameterCount { get; private set; }
        public string Identifier { get; private set; }

        public InternalExpression(string identifier, int parameterCount = 0)
        {
            this.Identifier = identifier;
            this.ParameterCount = parameterCount;

            this.ParameterNames = 
                Enumerable.Range(0, this.ParameterCount)
                .Select(i => Guid.NewGuid().ToString())
                .ToArray();

            this.Parameters =
                this.ParameterNames
                .Select(paramName => new Lexeme
                {
                    Type = LexemeType.Identifier,
                    Value = paramName
                } as ILexeme)
                .ToArray();
        }

        protected IExpression GetParameter(CSquaredEnvironment environment, int index)
        {
            IExpression expression;

            environment.Lookup(this.ParameterNames[index], out expression);

            return expression;
        }
    }
}