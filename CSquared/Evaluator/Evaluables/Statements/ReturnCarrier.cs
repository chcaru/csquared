using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class ReturnCarrier : 
        IExpression, 
        ICastable<Integer>,
        ICastable<Real>,
        ICastable<Array>,
        ICastable<CSquaredObject>,
        ICastable<Null>,
        ICastable<CSquaredString>,
        ICastable<LambdaExpression>,
        ICastable<IPrimary>
    {
        public IExpression EvaluatedReturnExpression { get; set; }

        public IExpression Evaluate(CSquaredEnvironment context)
        {
            return this.EvaluatedReturnExpression;
        }

        private T As<T>()
        {
            return this.EvaluatedReturnExpression.CastTo<T>();
        }

        Integer ICastable<Integer>.CastTo()
        {
            return this.As<Integer>();
        }

        Real ICastable<Real>.CastTo()
        {
            return this.As<Real>();
        }

        Array ICastable<Array>.CastTo()
        {
            return this.As<Array>();
        }

        CSquaredObject ICastable<CSquaredObject>.CastTo()
        {
            return this.As<CSquaredObject>();
        }

        Null ICastable<Null>.CastTo()
        {
            return this.As<Null>();
        }

        CSquaredString ICastable<CSquaredString>.CastTo()
        {
            return this.As<CSquaredString>();
        }

        LambdaExpression ICastable<LambdaExpression>.CastTo()
        {
            return this.As<LambdaExpression>();
        }

        IPrimary ICastable<IPrimary>.CastTo()
        {
            return this.As<IPrimary>();
        }
    }
}
