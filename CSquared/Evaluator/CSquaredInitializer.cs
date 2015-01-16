using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public static class CSquaredInitializer
    {
        public static void Initialize(CSquaredEnvironment enviroment)
        {
            InitializeInternalExpression<PrintExpression>(enviroment);
            InitializeInternalExpression<PrintlnExpression>(enviroment);
            InitializeInternalExpression<InputExpression>(enviroment);
            InitializeInternalExpression<EmptyExpression>(enviroment);
            InitializeInternalExpression<CountExpression>(enviroment);
            InitializeInternalExpression<InjectExpression>(enviroment);

            InitializeInternalExpression<CastExpression<Integer>>(enviroment, "toInt");
            InitializeInternalExpression<CastExpression<Real>>(enviroment, "toReal");
            InitializeInternalExpression<CastExpression<CSquaredBoolean>>(enviroment, "toBool");
            InitializeInternalExpression<CastExpression<CSquaredString>>(enviroment, "toString");

            InitializeInternalExpression<TypeCheckExpression<Integer>>(enviroment, "isInt");
            InitializeInternalExpression<TypeCheckExpression<Real>>(enviroment, "isReal");
            InitializeInternalExpression<TypeCheckExpression<CSquaredBoolean>>(enviroment, "isBool");
            InitializeInternalExpression<TypeCheckExpression<CSquaredString>>(enviroment, "isString");
            InitializeInternalExpression<TypeCheckExpression<CSquaredObject>>(enviroment, "isObject");
            InitializeInternalExpression<TypeCheckExpression<Array>>(enviroment, "isArray");
            InitializeInternalExpression<TypeCheckExpression<LambdaExpression>>(enviroment, "isLambda");
        }

        private static void InitializeInternalExpression<T>(CSquaredEnvironment environment, string identifier = null) 
            where T : InternalExpression, new()
        {
            var internalExpression = new T { Closure = environment.Extend() };
            environment.Set(identifier ?? internalExpression.Identifier, internalExpression);
        }
    }
}
