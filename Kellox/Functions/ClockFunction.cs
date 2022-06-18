using Kellox.Tokens;

namespace Kellox.Functions;

/// <summary>
/// Native Function Clock implemented in the host language C#
/// </summary>
internal class ClockFunction : IFunction
{
    public int Arity => 0;

    public object? Call(List<object?> arguments, Token paren) => DateTime.Now.Second;

    public override string ToString() => "native clock function";
}
