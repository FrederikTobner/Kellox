namespace Interpreter.Functions
{
    internal interface IFunction
    {
        /// <summary>
        /// Number of arguments
        /// </summary>
        public int Arity { get; }

        object? Call(List<object?> arguments);
    }
}
