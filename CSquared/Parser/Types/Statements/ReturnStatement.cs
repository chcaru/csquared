﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class ReturnStatement : IStatement
    {
        public IExpression ReturnExpression { get; set; }
    }
}