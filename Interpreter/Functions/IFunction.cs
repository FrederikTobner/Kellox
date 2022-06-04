namespace Interpreter.Functions
{
    internal interface IFunction
    {
        /// <summary>
        /// The arity of the function -> fancy term for the number of arguments a function expects
        /// </summary>
        public int Arity { get; }

        /// <summary>
        /// Calls the function
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        object? Call(List<object?> arguments);
    }
}
