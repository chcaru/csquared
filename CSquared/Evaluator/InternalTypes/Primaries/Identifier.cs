using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class Identifier : Primary<string, Identifier>
    {
        public override IExpression Initialize(CSquaredEnvironment environment)
        {
            return this;
        }

        public override IExpression Evaluate(CSquaredEnvironment environment)
        {
            IExpression expression;

            if (!environment.Lookup(this.Value, out expression))
            {
                throw new Exception("Attempted to access undefined variable " + this.Value);
            }

            return expression.Evaluate(environment);
        }

        private T EvaluateTo<T>(CSquaredEnvironment environment) where T : class, IPrimary
        {
            var identifierValue = this.Evaluate(environment).CastTo<T>();

            if (identifierValue.IsNull())
            {
                throw new Exception("Variable " + this.Value + " does not support primary operations.");
            }

            return identifierValue;
        }

        public override IPrimary Add(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.Add(environment, expression);
        }

        public override IPrimary Subtract(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.Subtract(environment, expression);
        }

        public override IPrimary Multiply(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.Multiply(environment, expression);
        }

        public override IPrimary Divide(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.Divide(environment, expression);
        }

        public override IExpression Access(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.Access(environment, expression);
        }

        public override CSquaredBoolean And(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<CSquaredBoolean>(environment);

            return identifierValue.And(environment, expression);
        }

        public override CSquaredBoolean Or(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<CSquaredBoolean>(environment);

            return identifierValue.Or(environment, expression);
        }

        public override CSquaredBoolean Equal(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.Equal(environment, expression);
        }

        public override CSquaredBoolean NotEqual(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.NotEqual(environment, expression);
        }

        public override CSquaredBoolean Not(CSquaredEnvironment environment)
        {
            var identifierValue = this.EvaluateTo<CSquaredBoolean>(environment);

            return identifierValue.Not(environment);
        }

        public override CSquaredBoolean GreaterThan(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.GreaterThan(environment, expression);
        }

        public override CSquaredBoolean GreaterThanOrEqual(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.GreaterThanOrEqual(environment, expression);
        }

        public override CSquaredBoolean LessThan(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.LessThan(environment, expression);
        }

        public override CSquaredBoolean LessThanOrEqual(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.LessThanOrEqual(environment, expression);
        }

        public override IExpression InsertLeft(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            if (identifierValue is CSquaredObject)
            {
                return identifierValue.InsertLeft(environment, expression);
            }

            var expressionResult = expression.Evaluate(environment) as CSquaredObject;

            if (expressionResult.IsNull())
            {
                throw new Exception("Attempted to move the last element of a non-object to variable");
            }

            var front = expressionResult.RemoveFromFront();

            if (!environment.Update(this.Value, front))
            {
                throw new Exception("Attempted to move the first element of an object to an undeclared variable");
            }

            return front;
        }

        public override IExpression InsertRight(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.Evaluate(environment);

            if (identifierValue is CSquaredObject)
            {
                return (identifierValue as CSquaredObject).InsertRight(environment, expression);
            }

            var expressionResult = expression.Evaluate(environment) as CSquaredObject;

            if (expressionResult.IsNull())
            {
                throw new Exception("Attempted to move the last element of a non-object to variable");
            }

            expressionResult.AddToFront(identifierValue);

            return identifierValue;
        }

        public override IExpression Index(CSquaredEnvironment environment, IExpression expression)
        {
            var identifierValue = this.EvaluateTo<IPrimary>(environment);

            return identifierValue.Index(environment, expression);
        }
    }
}
