using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public interface IIndexable
    {
        void SetIndex(CSquaredEnvironment environment, IExpression indexExpression, IExpression expression);
    }
}
