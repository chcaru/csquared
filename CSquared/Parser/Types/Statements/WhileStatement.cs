using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class WhileStatement : IStatement
    {
        public IExpression ConditionalExpression { get; set; }
        public IEnumerable<IStatement> Body { get; set; }
    }
}
