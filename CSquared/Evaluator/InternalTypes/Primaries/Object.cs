using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class CSquaredObject : 
        Primary<CSquaredEnvironment, CSquaredObject>, 
        IEnumerable<IExpression>, 
        IIndexable
    {
        private IEnumerable<IExpression> Enumerator
        {
            get { return this.Value.ConstituentsOrder.Select(c => c.Expression); }
        }

        public override IExpression Initialize(CSquaredEnvironment environment)
        {
            var objectEnvironment = environment.Extend();

            var newObject = new CSquaredObject
            {
                VariableDeclarations = this.VariableDeclarations,
                Value = objectEnvironment
            };

            foreach (var variableDeclaration in this.VariableDeclarations)
            {
                variableDeclaration.Evaluate(objectEnvironment);
            }

            return newObject;
        }

        public void AddToEnd(IExpression expression)
        {
            this.Value.AddAnonymous(expression);
        }

        public void AddToFront(IExpression expression)
        {
            this.Value.AddAnonymous(expression, false);
        }

        public IExpression RemoveFromEnd()
        {
            var last = this.Value.ConstituentsOrder.Last;

            this.Value.ConstituentsOrder.RemoveLast();

            return last.Value.Expression;
        }

        public IExpression RemoveFromFront()
        {
            var first = this.Value.ConstituentsOrder.First;

            this.Value.ConstituentsOrder.RemoveFirst();

            return first.Value.Expression;
        }

        IEnumerator<IExpression> IEnumerable<IExpression>.GetEnumerator()
        {
            return this.Enumerator.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Enumerator.GetEnumerator();
        }

        public override IExpression Access(CSquaredEnvironment environment, IExpression expression)
        {
            if (expression is Identifier)
            {
                var identifier = expression as Identifier;

                return identifier.Evaluate(this.Value) as IExpression;
            }

            var binaryExpression = expression as BinaryOperationExpression;

            if (binaryExpression.IsNull())
            {
                return expression.Evaluate(this.Value) as IExpression;
            }

            if (binaryExpression.BinaryOperator.Type != LexemeType.Dot)
            {
                return binaryExpression.Evaluate(this.Value) as IExpression;
            }

            var accessIdentifier = binaryExpression.PreExpression as Identifier;

            if (accessIdentifier.IsNull())
            {
                throw new Exception("Attempted to access an unsupported type");
            }

            var identifierExpression = accessIdentifier.Evaluate(this.Value) as IPrimary;

            if (identifierExpression.IsNull())
            {
                throw new Exception("Attempted to access an unsupported type: " + accessIdentifier.Value);
            }

            return identifierExpression.Access(this.Value, binaryExpression.PostExpression);
        }

        public override IExpression InsertLeft(CSquaredEnvironment environment, IExpression expression)
        {
            IExpression expressionResult = expression;

            expressionResult = expression.Evaluate(environment);

            this.AddToEnd(expressionResult);

            return expressionResult;
        }

        public override IExpression InsertRight(CSquaredEnvironment environment, IExpression expression)
        {
            var expressionResult = expression as Identifier;

            if (expressionResult.IsNull())
            {
                throw new Exception("Attempted to move the last element of an object to a non-variable");
            }

            IExpression identifierExpression;

            if (!environment.Lookup(expressionResult.Value, out identifierExpression))
            {
                throw new Exception("Attempted to move the last element of an object to an undeclared variable");
            }

            var end = this.RemoveFromEnd();

            if (!environment.Update(expressionResult.Value, end))
            {
                throw new Exception("Fatal Error. Identifier " + expressionResult.Value + " existed previously, but no longer exists.");
            }

            return end;
        }

        public void SetIndex(CSquaredEnvironment environment, IExpression indexExpression, IExpression expression)
        {
            var index = indexExpression.Evaluate(environment).CastTo<CSquaredString>().Value;

            var evaluatedExpression = expression.Evaluate(environment);

            this.Value.Set(index, evaluatedExpression);
        }

        public override IExpression Index(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedIndexExpression = expression.Evaluate(environment).CastTo<CSquaredString>();

            IExpression indexedExpression;

            return
                this.Value.Lookup(evaluatedIndexExpression.Value, out indexedExpression) ?
                    indexedExpression :
                    new Null();
        }

        public override CSquaredBoolean Equal(CSquaredEnvironment environment, IExpression expression)
        {
            return CSquaredBoolean.From(expression == this);
        }

        public override string ToString()
        {
            return "Object (" + this.GetHashCode() + ")";
        }
    }
}
