using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class LambdaCall : IExpression
    {
        public ILexeme Identifier { get; set; }
        public IEnumerable<IExpression> Arguments { get; set; }
    }
}
