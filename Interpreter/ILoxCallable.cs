namespace Interpreter
{
    internal interface ILoxCallable
    {
        public int Arity { get; }
        object? Call(List<object?> arguments);
    }
}
