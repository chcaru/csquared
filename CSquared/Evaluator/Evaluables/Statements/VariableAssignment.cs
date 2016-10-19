using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
	public partial class VariableAssignment : IStatement
	{
		public IExpression Evaluate(CSquaredEnvironment environment)
		{
			var variableIdentifier = this.Identifier.Value.ToString();

			var expression =
				 this.Expression.IsNull() ?
					 new Null() :
					 this.Expression.Evaluate(environment);


			if (!this.IndexExpression.IsNull())
			{
				IExpression indexableExpression = null;

				if (!environment.Lookup(variableIdentifier, out indexableExpression))
				{
					throw new Exception("Attempted to assign value to undeclared variable " + variableIdentifier);
				}

				var indexable = indexableExpression.CastTo<IIndexable>();

				indexable.SetIndex(environment, this.IndexExpression, expression);
			}
			else if (!environment.Update(variableIdentifier, expression))
			{
				var variableDeclaration = new VariableDeclaration
				{
					IdentifierDeclaration = new IdentifierDeclaration
					{
						Identifier = this.Identifier,
						ScopeEscalated = false
					},
					Expression = this.Expression
				};

				return variableDeclaration.Evaluate(environment);
			}

			return new Null();
		}
	}
}