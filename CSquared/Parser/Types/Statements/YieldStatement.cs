using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class YieldStatement : IStatement
    {
        public IExpression YieldExpression { get; set; }
        public ILexeme YieldTo { get; set; }
    }
}
