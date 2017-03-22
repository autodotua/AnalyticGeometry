using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expressao;
using Projetasoft.Expression;
using System.Diagnostics;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Stopwatch();
            c.Start();
           var r= new Resolver("x+2");
          for(int i=1;i<=1000;i++)
            {
                r["x"] = i;
                r.SolverExpression();
            }
            c.Stop();
            Console.WriteLine(c.ElapsedMilliseconds);
           Console.ReadKey();
        }
    }
}
