using System.Text;

namespace Interpreter
{
    internal abstract class Expression
    {
        public abstract override string ToString();

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
    }
}
