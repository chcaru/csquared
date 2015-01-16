using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class VariableDeclaration : IStatement
    {
        public IdentifierDeclaration IdentifierDeclaration { get; set; }
        public IExpression Expression { get; set; }
    }
}
