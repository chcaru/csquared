﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public interface IStatement : IEvaluable<IExpression, CSquaredEnvironment> { }
}