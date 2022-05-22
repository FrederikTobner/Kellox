using System.Text;

namespace Interpreter
{
    internal abstract class Expression
    {
        public abstract override string ToString();

        public abstract object? EvaluateExpression();

        protected static string Parenthesize(string name, params Expression[] expressions)
        {
            StringBuilder builder = new();

            builder.Append('(').Append(name);
            foreach (Expression expression in expressions)
            {
                builder.Append(' ');
                builder.Append(expression.ToString());
            }
            builder.Append(')');

            return builder.ToString();
        }

        protected static bool IsTruthy(object obj) => obj switch
        {
            null => false,
            bool b => b,
            _ => true
        };

        protected static bool IsEqual(object? obj1, object? obj2)
        {
            if (obj1 is null && obj2 is null)
            {
                return false;
            }
            if (obj1 is null)
            {
                return false;
            }
            return obj1.Equals(obj2);
        }
    }
}
