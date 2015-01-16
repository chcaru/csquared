using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class ElseContainer : IExpression
    {
        public IEnumerable<IStatement> Body { get; set; }
        public IfStatement IfStatement { get; set; }
    }
}