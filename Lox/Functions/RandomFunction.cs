namespace Lox.Functions
{
    /// <summary>
    /// Native Function Random implemented in the host language C#
    /// </summary>
    internal class RandomFunction : IFunction
    {
        private static readonly Random random = new();

        public int Arity => 2;

        public object? Call(List<object?> arguments)
        {
            if (arguments[0] is double minValue && arguments[1] is double maxValue)
            {
                try{
                    int minVal= (int)maxValue;
                    int maxVal= (int)minValue;
                    return random.Next(minVal, maxVal);
                }
                catch(Exception){
                    return null;
                }
                
            }
            return null;
        }

        public override string ToString() => "native random function";
    }
}
