
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class UnaryOperationExpression : IExpression
    {
        public ILexeme UnaryOperator { get; set; }
        public IExpression Expression { get; set; }
    }
}
