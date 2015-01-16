using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class BinaryOperationExpression : IExpression
    {
        public IExpression Evaluate(CSquaredEnvironment environment)
        {
            IPrimary preExpressionAsPrimary;
            
            if (this.PreExpression is Identifier)
            {
                preExpressionAsPrimary = this.PreExpression as IPrimary;
            }
            else
            {
                preExpressionAsPrimary = this.PreExpression.Evaluate(environment).CastTo<IPrimary>();
            }

            if (preExpressionAsPrimary.IsNull())
            {
                throw new Exception("Attempted to perform a primary operation on a non-primary.");
            }

            switch(this.BinaryOperator.Type)
            {
                case LexemeType.Plus: 
                    return preExpressionAsPrimary.Add(environment, this.PostExpression);
                case LexemeType.Minus: 
                    return preExpressionAsPrimary.Subtract(environment, this.PostExpression);
                case LexemeType.Times: 
                    return preExpressionAsPrimary.Multiply(environment, this.PostExpression);
                case LexemeType.Divides: 
                    return preExpressionAsPrimary.Divide(environment, this.PostExpression);
                case LexemeType.Dot: 
                    return preExpressionAsPrimary.Access(environment, this.PostExpression);
                case LexemeType.DoubleEqual: 
                    return preExpressionAsPrimary.Equal(environment, this.PostExpression);
                case LexemeType.NotEqual: 
                    return preExpressionAsPrimary.NotEqual(environment, this.PostExpression);
                case LexemeType.LessThanOrEqual:
                    return preExpressionAsPrimary.LessThanOrEqual(environment, this.PostExpression);
                case LexemeType.LessThan:
                    return preExpressionAsPrimary.LessThan(environment, this.PostExpression);
                case LexemeType.GreaterThanOrEqual:
                    return preExpressionAsPrimary.GreaterThanOrEqual(environment, this.PostExpression);
                case LexemeType.GreaterThan:
                    return preExpressionAsPrimary.GreaterThan(environment, this.PostExpression);
                case LexemeType.And:
                    return preExpressionAsPrimary.And(environment, this.PostExpression);
                case LexemeType.Or:
                    return preExpressionAsPrimary.Or(environment, this.PostExpression);
                case LexemeType.LeftArrow:
                    return preExpressionAsPrimary.InsertLeft(environment, this.PostExpression);
                case LexemeType.RightArrow:
                    return preExpressionAsPrimary.InsertRight(environment, this.PostExpression);
                case LexemeType.OpenSquareBracket:
                    return preExpressionAsPrimary.Index(environment, this.PostExpression);
            }

            return new Null();
        }
    }
}
