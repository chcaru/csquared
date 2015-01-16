using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class CSquaredObject : IExpression, IEnumerable<IExpression>
    {
        public IEnumerable<IStatement> VariableDeclarations { get; set; }
    }
}
