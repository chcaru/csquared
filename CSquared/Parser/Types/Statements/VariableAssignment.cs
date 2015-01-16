using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class VariableAssignment : IStatement
    {
        public ILexeme Identifier { get; set; }
        public IExpression Expression { get; set; }
        public IExpression IndexExpression { get; set; }
    }
}