using Kellox.Exceptions;
using Kellox.Tokens;

namespace Kellox.Functions
{
    /// <summary>
    /// Native Function Random implemented in the host language C#
    /// </summary>
    internal class RandomFunction : IFunction
    {
        // Native Pseudo Random Number Generator of the .Net framework
        private static readonly Random random = new();

        public int Arity => 2;

        public object? Call(List<object?> arguments, Token paren)
        {
            if (arguments[0] is double minValue && arguments[1] is double maxValue)
            {
                if (minValue > maxValue)
                {
                    throw new RunTimeError(paren, "minValue must be greater than maxvalue");
                }
                try
                {
                    int minVal = (int)minValue;
                    int maxVal = (int)maxValue;
                    return random.Next(minVal, maxVal);
                }
                catch (Exception)
                {
                    throw new RunTimeError(paren, "Arguments for random call out of bounds, maxValue is " + int.MaxValue + " and minValue" + int.MinValue);
                }

            }
            throw new RunTimeError(paren, "Can't call random function with anything that is not a Number");
        }

        public override string ToString() => "native random function";
    }
}
