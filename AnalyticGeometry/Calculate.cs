using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSScriptControl;

namespace AnalyticGeometry
{
    static class Calculate
    {
        //计算表达式，错误返回null
        /// <summary>
        /// 计算表达式，错误返回null
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="argument">参数</param>
        /// <param name="num">参数值</param>
        /// <returns></returns>
        public static string Eval(string expression, string argument, string num)
        {
            MSScriptControl.ScriptControl script = new MSScriptControl.ScriptControlClass();
            script.Language = "JavaScript";
            try
            {
               // return (new SimpleExpressionEvaluator.ExpressionEvaluator().Evaluate(ReplaceExpressionWithArguement(expression, argument, num))).ToString();
                return script.Eval(ReplaceExpressionWithArguement(expression, argument, num)).ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
        //计算表达式，错误返回null
        /// <summary>
        /// 计算表达式，错误返回null
        /// </summary>
        /// <param name="expression">表达式（不带参数）</param>
        /// <returns></returns>
        public static string Eval(string expression)
        {
            MSScriptControl.ScriptControl script = new MSScriptControl.ScriptControlClass();
            script.Language = "JavaScript";
            
            try
            {
              // return (new SimpleExpressionEvaluator.ExpressionEvaluator().Evaluate(expression)).ToString();
                return script.Eval(ReplaceExpression(expression)).ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
        //计算带参数的表达式前预先整理表达式
        /// <summary>
        /// 计算带参数的表达式前预先整理表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="argument">参数</param>
        /// <returns></returns>
        public static string ReplaceExpressionPreliminary(string expression, string argument)
        {
            string newExpression = expression.Replace("Math.", "") ;
            for (int k = 65; k <= 122; k++)
            {
                if (k >= 91 && k <= 96)
                {
                    continue;
                }
                for (int j = 0; j <= 9; j++)
                {
                    newExpression = newExpression.Replace(j.ToString() + ((char)k).ToString(), j.ToString() + "*" + ((char)k).ToString());
                }
                newExpression = newExpression.Replace(")" + ((char)k).ToString(), ")*" + ((char)k).ToString());
            }
            for (int j = 0; j <= 9; j++)
            {
                newExpression = newExpression.Replace(j.ToString() + "(", j.ToString() + "*(");
            }
            newExpression = newExpression.Replace(")(", ")*(");
            string[] mathFunc = { "sqrt", "pow", "log", "exp", "asin", "acos", "atan", "sin", "cos", "tan", "random", "round", "floor", "ceil","min","max" };
           
            int i=0;
            foreach (string eachMathFunc in mathFunc)
            {
                i++;
                newExpression = newExpression.Replace(eachMathFunc, "?????" + i.ToString());
            }
            return newExpression;
        }
        //计算带参数的表达式的第二步替换
        private static string ReplaceExpressionWithArguement(string expression, string argument, string num)
        {
            string[] mathFunc = { "sqrt", "pow", "log", "exp", "asin", "acos", "atan", "sin", "cos", "tan", "random", "round", "floor", "ceil", "min", "max" };

            string newExpression = expression.Replace(argument, num);
            for (int j = mathFunc.Length - 1; j >= 0; j--)
            {
                newExpression = newExpression.Replace("?????" + (j + 1).ToString(), "Math." + mathFunc[j]);
            }

            return (newExpression.Replace("--","+").Replace("++","+"));
        }
        //计算普通表达式的替换
        private static string ReplaceExpression(string expression)
        {
            string newExpression = expression.Replace("Math.", "");
            for (int k = 65; k <= 122; k++)
            {
                if (k >= 91 && k <= 96)
                {
                    continue;
                }
                for (int j = 0; j <= 9; j++)
                {
                    newExpression = newExpression.Replace(j.ToString() + ((char)k).ToString(), j.ToString() + "*" + ((char)k).ToString());
                }
                newExpression = newExpression.Replace(")" + ((char)k).ToString(), ")*" + ((char)k).ToString());
            }
            for (int j = 0; j <= 9; j++)
            {
                newExpression = newExpression.Replace(j.ToString() + "(", j.ToString() + "*(");
            }
            newExpression = newExpression.Replace(")(", ")*(");


            string[] mathFunc = { "sqrt", "pow", "log", "exp", "asin", "acos", "atan", "sin", "cos", "tan", "random", "round", "floor", "ceil", "min", "max" };

            int i = 0;
            foreach (string eachMathFunc in mathFunc)
            {
                i++;
                newExpression = newExpression.Replace(eachMathFunc, "Math." + eachMathFunc);
            }
            return newExpression;
        }

    }
}
