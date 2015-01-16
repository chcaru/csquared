using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class LockStatement : IStatement
    {
        public ILexeme Identifier { get; set; }
        public IEnumerable<IStatement> Body { get; set; }
    }
}