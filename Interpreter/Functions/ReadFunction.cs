namespace Interpreter.Functions
{
    internal class ReadFunction : IFunction
    {
        public int Arity => 0;

        public object? Call(List<object?> arguments)
        {
            string? text = Console.ReadLine();
            return text is null ? "" : text;
        }
    }
}
