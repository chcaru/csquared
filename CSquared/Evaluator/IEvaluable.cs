using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public interface IEvaluable<T>
    {
        void Evaluate(T context);
    }

    public interface IEvaluable<T, J>
    {
        T Evaluate(J context);
    }
}
