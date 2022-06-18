using Kellox.Exceptions;
using Kellox.Tokens;

namespace Kellox.Functions
{
    /// <summary>
    /// Native Function Random implemented in the host language C#
    /// </summary>
    internal class RandomFunction : IFunction
    {
        private static readonly Random random = new();

        public int Arity => 2;

        public object? Call(List<object?> arguments, Token paren)
        {
            if (arguments[0] is double minValue && arguments[1] is double maxValue)
            {
                try
                {
                    int minVal = (int)minValue;
                    int maxVal = (int)maxValue;
                    return random.Next(minVal, maxVal);
                }
                catch (Exception)
                {
                    throw new RunTimeError(paren, "Arguments for random call out of bounds");
                }

            }
            return null;
        }

        public override string ToString() => "native clock function";
    }
}
