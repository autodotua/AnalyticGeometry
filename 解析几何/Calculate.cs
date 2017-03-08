using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSScriptControl;

namespace 解析几何
{
    static class Calculate
    {
       /// <summary>
        /// 计算表达式，错误返回null
       /// </summary>
       /// <param name="equation">表达式</param>
       /// <param name="argument">变量标识符</param>
       /// <param name="num">变量</param>
       /// <returns></returns>
        public static string eval(string equation, string argument, string num)//计算表达式，错误返回null
        {
            MSScriptControl.ScriptControl script = new MSScriptControl.ScriptControlClass();
            script.Language = "JavaScript";
            try
            {
                
                return script.Eval(replace2(equation, argument, num)).ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string replace1(string equation, string argument)
        {
            string newEquation = equation.Replace("Math.", "") ;
            for (int k = 65; k <= 122; k++)
            {
                if (k >= 91 && k <= 96)
                {
                    continue;
                }
                for (int j = 0; j <= 9; j++)
                {
                    newEquation = newEquation.Replace(j.ToString() + ((char)k).ToString(), j.ToString() + "*" + ((char)k).ToString());
                }
                newEquation = newEquation.Replace(")" + ((char)k).ToString(), ")*" + ((char)k).ToString());
            }
            for (int j = 0; j <= 9; j++)
            {
                newEquation = newEquation.Replace(j.ToString() + "(", j.ToString() + "*(");
            }
            newEquation = newEquation.Replace(")(", ")*(");
            string[] mathFunc = { "sqrt", "pow", "log", "exp", "asin", "acos", "atan", "sin", "cos", "tan", "random", "round", "floor", "ceil","min","max" };
           
            int i=0;
            foreach (string eachMathFunc in mathFunc)
            {
                i++;
                newEquation = newEquation.Replace(eachMathFunc, "?????" + i.ToString());
            }
            return newEquation;
        }
        public static string replace2(string equation, string argument, string num)
        {
            string[] mathFunc = { "sqrt", "pow", "log", "exp", "asin", "acos", "atan", "sin", "cos", "tan", "random", "round", "floor", "ceil", "min", "max" };

            string newEquation = equation.Replace(argument, num);
            for (int j = mathFunc.Length - 1; j >= 0; j--)
            {
                newEquation = newEquation.Replace("?????" + (j + 1).ToString(), "Math." + mathFunc[j]);
            }

            return (newEquation.Replace("--","+").Replace("++","+"));
        }

        public static string eval(string equation)//计算表达式，错误返回null
        {
            MSScriptControl.ScriptControl script = new MSScriptControl.ScriptControlClass();
            script.Language = "JavaScript";


            try
            {
                return script.Eval(replace(equation)).ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string replace(string equation)
        {
            string newEquation = equation.Replace("Math.", "");
            for (int k = 65; k <= 122; k++)
            {
                if (k >= 91 && k <= 96)
                {
                    continue;
                }
                for (int j = 0; j <= 9; j++)
                {
                    newEquation = newEquation.Replace(j.ToString() + ((char)k).ToString(), j.ToString() + "*" + ((char)k).ToString());
                }
                newEquation = newEquation.Replace(")" + ((char)k).ToString(), ")*" + ((char)k).ToString());
            }
            for (int j = 0; j <= 9; j++)
            {
                newEquation = newEquation.Replace(j.ToString() + "(", j.ToString() + "*(");
            }
            newEquation = newEquation.Replace(")(", ")*(");


            string[] mathFunc = { "sqrt", "pow", "log", "exp", "asin", "acos", "atan", "sin", "cos", "tan", "random", "round", "floor", "ceil", "min", "max" };

            int i = 0;
            foreach (string eachMathFunc in mathFunc)
            {
                i++;
                newEquation = newEquation.Replace(eachMathFunc, "Math." + eachMathFunc);
            }
            return newEquation;
        }

    }
}
