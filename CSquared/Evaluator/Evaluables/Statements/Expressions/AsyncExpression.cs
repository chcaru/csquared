using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public partial class AsyncExpression : IExpression, IInitializable
    {
        private Task<IExpression> EvaluationTask { get; set; }

        public IExpression Initialize(CSquaredEnvironment environment)
        {
            return new AsyncExpression
            {
                Expression = this.Expression
            };
        }

        public IExpression Evaluate(CSquaredEnvironment environment)
        {
            if (!this.EvaluationTask.IsNull())
            {
                if (this.EvaluationTask.Status == TaskStatus.Running)
                {
                    this.EvaluationTask.Wait();
                }

                return this.EvaluationTask.Result ?? new Null();
            }

            this.EvaluationTask = Task.Run(
                () => this.Expression.Evaluate(environment)
            );

            return this;
        }
    }
}