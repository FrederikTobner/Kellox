namespace Lox.Functions;

/// <summary>
/// Native Function Clear implemented in the host language C# -> CLears the console
/// </summary>
internal class ClearFunction : IFunction
{
    public int Arity => 0;

    public object? Call(List<object?> arguments)
    {
        Console.Clear();
        return null;
    }

    public override string ToString() => "native clear function";
}
