namespace Interpreter.Functions
{
    internal class ClearFunction : IFunction
    {
        public int Arity => 0;

        public object? Call(List<object?> arguments)
        {
            Console.Clear();
            return null;
        }
    }
}
