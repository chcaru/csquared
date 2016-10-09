using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class VariableDeclaration : IStatement
    {
        public virtual IExpression Evaluate(CSquaredEnvironment environment)
        {
            var variableIdentifier = this.IdentifierDeclaration.Identifier.Value.ToString();

            var declaringEnvironment =
                this.IdentifierDeclaration.ScopeEscalated ?
                    environment.GetParent() :
                    environment;

            var expression = this.Expression;

            IInitializable initializableExpression;

            if (expression.TryCastTo<IInitializable>(out initializableExpression))
            {
                expression = initializableExpression.Initialize(declaringEnvironment);
            }

            expression =
                expression.IsNull() ?
                    new Null() :
                    expression.Evaluate(environment);
            
            declaringEnvironment.Set(variableIdentifier, expression);

            return new Null();
        }
    }
}