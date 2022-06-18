using Kellox.Classes;
using Kellox.Tokens;

namespace Kellox.Functions
{
    internal class TypeOfFunction : IFunction
    {
        public int Arity => 1;

        public object? Call(List<object?> arguments, Token paren)
        {
            if (arguments[0] is string)
            {
                return "String";
            }
            if (arguments[0] is double)
            {
                return "Number";
            }
            if (arguments[0] is bool)
            {
                return "Boolean";
            }
            if (arguments[0] is KelloxInstance kelloxInstance)
            {
                return kelloxInstance.KelloxClass.ToString();
            }
            return "Undefiended";
        }
    }
}
