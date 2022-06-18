using Kellox.Tokens;

namespace Kellox.Functions;

/// <summary>
/// Native Function Clear implemented in the host language C#
/// </summary>
internal class ClearFunction : IFunction
{
    public int Arity => 0;

    public object? Call(List<object?> arguments, Token paren)
    {
        Console.Clear();
        return null;
    }
}
