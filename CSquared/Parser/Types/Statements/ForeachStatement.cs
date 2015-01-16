using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class ForeachStatement : IStatement
    {
        public ILexeme OutIdentifier { get; set; }
        public ILexeme InIdentifier { get; set; }
        public IEnumerable<IStatement> Body { get; set; }
    }
}
