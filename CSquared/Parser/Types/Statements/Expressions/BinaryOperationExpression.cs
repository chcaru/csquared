
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class BinaryOperationExpression : IExpression
    {
        public IExpression PreExpression { get; set; }
        public ILexeme BinaryOperator { get; set; }
        public IExpression PostExpression { get; set; }
    }
}
