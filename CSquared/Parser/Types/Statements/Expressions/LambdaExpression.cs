using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class LambdaExpression : IExpression
    {
        public virtual IEnumerable<ILexeme> Parameters { get; set; }
        public virtual IEnumerable<IStatement> Body { get; set; }
    }
}
