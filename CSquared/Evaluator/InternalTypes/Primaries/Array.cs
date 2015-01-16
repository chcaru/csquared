using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class Array : 
        Primary<IExpression[], Array>, 
        IInitializable, 
        IIndexable, 
        IEnumerable<IExpression>
    {
        //private CSquaredEnvironment Closure { get; set; }

        public override IExpression Initialize(CSquaredEnvironment environment)
        {
            int arraySize =
                this.SizeExpression.IsNull() ?
                    this.Expressions.Count() :
                    this.SizeExpression.Evaluate(environment).CastTo<Integer>().Value;

            var newArray = new Array
            {
                //Closure = environment,
                Expressions = this.Expressions,
                SizeExpression = this.SizeExpression,
                Value = new IExpression[arraySize]
            };

            var index = 0;

            if (!this.Expressions.IsEmpty())
            {
                foreach (var expression in this.Expressions)
                {
                    newArray.Value[index] = expression.Evaluate(environment);
                    index++;
                }
            }

            for (; index < arraySize; index++)
            {
                newArray.Value[index] = new Null();
            }

            return newArray;
        }

        public IExpression GetIndex(int index)
        {
            if (this.Value.IsNull())
            {
                throw new Exception("Attempt to access uninitialized array");
            }

            return this.Value[index];
        }

        public void SetIndex(CSquaredEnvironment environment, IExpression indexExpression, IExpression expression)
        {
            if (this.Value.IsNull())
            {
                throw new Exception("Attempt to access uninitialized array");
            }

            var index = indexExpression.Evaluate(environment).CastTo<Integer>().Value;

            var evaluatedExpression = expression.Evaluate(environment);

            this.Value[index] = evaluatedExpression;
        }

        public override IExpression Index(CSquaredEnvironment environment, IExpression expression)
        {
            var evaluatedIndexExpression = expression.Evaluate(environment).CastTo<Integer>();

            var indexedExpression = this.GetIndex(evaluatedIndexExpression.Value);
            
            return indexedExpression;//.Evaluate(this.Closure);
        }

        IEnumerator<IExpression> IEnumerable<IExpression>.GetEnumerator()
        {
            return (this.Value as IEnumerable<IExpression>).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Value.GetEnumerator();
        }
    }
}

