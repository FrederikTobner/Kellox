namespace Kellox.Functions;

/// <summary>
/// Native Function Clear implemented in the host language C#
/// </summary>
internal class ClearFunction : IFunction
{
    public int Arity => 0;

    public object? Call(List<object?> arguments)
    {
        Console.Clear();
        return null;
    }
}
