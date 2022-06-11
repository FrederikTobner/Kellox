namespace Lox.Functions;

/// <summary>
/// Native wait-command that is used to wait a certain amount of seconds
/// </summary>
internal class WaitFunction : IFunction
{
    public int Arity => 1;

    public object? Call(List<object?> arguments)
    {
        if (arguments[0] is double seconds)
        {
            Thread.Sleep((int)(1000 * seconds));

        }
        return null;

    }
}
