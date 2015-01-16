using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class Evaluator
    {
        private CSquaredEnvironment GlobalEnvironment { get; set; }
        
        public Evaluator()
        {
            this.ResetEnvironment();
        }

        public Evaluator(CSquaredEnvironment envrionment)
        {
            this.GlobalEnvironment = envrionment;
        }

        public void ResetEnvironment()
        {
            this.GlobalEnvironment = new CSquaredEnvironment(null);
            CSquaredInitializer.Initialize(this.GlobalEnvironment);
        }

        public void Evaluate(Program program)
        {
            foreach (var statement in program.Statements)
            {
                this.Evaluate(statement);
            }
        }

        public void Evaluate(IStatement statement)
        {
            statement.Evaluate(this.GlobalEnvironment);
        }
    }
}
