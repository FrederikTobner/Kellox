namespace Lox.Functions;

/// <summary>
/// Native Function Read implemented in the host language C#
/// </summary>
internal class ReadFunction : IFunction
{
    public int Arity => 0;

    public object? Call(List<object?> arguments)
    {
        string? text = Console.ReadLine();
        return text is not null ? text : string.Empty;
    }

    public override string ToString() => "native read function";
}
