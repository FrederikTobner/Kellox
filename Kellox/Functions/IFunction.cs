namespace Kellox.Functions;

internal interface IFunction
{
    /// <summary>
    /// The arity of the function -> fancy term for the number of arguments a function expects
    /// </summary>
    public int Arity { get; }

    /// <summary>
    /// Calls the function
    /// </summary>
    /// <param name="arguments">Arguments used when the function was called</param>
    object? Call(List<object?> arguments);
}
