using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class IdentifierDeclaration
    {
        public bool ScopeEscalated { get; set; }
        public ILexeme Identifier { get; set; }
    }
}
