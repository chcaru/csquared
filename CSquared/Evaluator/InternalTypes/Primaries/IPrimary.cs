using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public interface IPrimary : IExpression
    {
        IPrimary Add(CSquaredEnvironment environment, IExpression expression);
        IPrimary Subtract(CSquaredEnvironment environment, IExpression expression);
        IPrimary Multiply(CSquaredEnvironment environment, IExpression expression);
        IPrimary Divide(CSquaredEnvironment environment, IExpression expression);
        CSquaredBoolean LessThanOrEqual(CSquaredEnvironment environment, IExpression expression);
        CSquaredBoolean GreaterThanOrEqual(CSquaredEnvironment environment, IExpression expression);
        CSquaredBoolean LessThan(CSquaredEnvironment environment, IExpression expression);
        CSquaredBoolean GreaterThan(CSquaredEnvironment environment, IExpression expression);
        CSquaredBoolean Equal(CSquaredEnvironment environment, IExpression expression);
        CSquaredBoolean And(CSquaredEnvironment environment, IExpression expression);
        CSquaredBoolean Or(CSquaredEnvironment environment, IExpression expression);
        CSquaredBoolean NotEqual(CSquaredEnvironment environment, IExpression expression);
        CSquaredBoolean Not(CSquaredEnvironment envrionment);
        IExpression Access(CSquaredEnvironment environment, IExpression expression);
        IExpression InsertLeft(CSquaredEnvironment environment, IExpression expression);
        IExpression InsertRight(CSquaredEnvironment environment, IExpression expression);
        IExpression Index(CSquaredEnvironment environment, IExpression expression);
    }
}
