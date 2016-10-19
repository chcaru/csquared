using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSquared;

namespace Interpreter
{
    class Interpreter
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
				throw new Exception("Program name required as command line arguement!");
            }

            var parser = new Parser(args[0]);
            var evaluator = new Evaluator();
            
            try
            {
                var program = parser.Parse();
                evaluator.Evaluate(program);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
