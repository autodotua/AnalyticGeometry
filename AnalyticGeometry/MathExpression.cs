using Projetasoft.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticGeometry
{
    class MathExpression
    {
        string expression;
        string parameter;
        Resolver rsv;
        public MathExpression(string _expression,string _parameter)
        {
            expression = _expression;
            parameter = _parameter;
            rsv = new Resolver(_expression);
        }
        public double Eval(double x)
        {
            rsv[parameter] = x;
            return rsv.SolverExpression();
        }
    }
}
