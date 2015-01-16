using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class Array : IExpression
    {
        public IEnumerable<IExpression> Expressions { get; set; }
        public IExpression SizeExpression { get; set; }
    }
}
