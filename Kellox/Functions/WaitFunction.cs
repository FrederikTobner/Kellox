using Lox.Interpreter.Exceptions;

namespace Lox.Functions;

/// <summary>
/// Native wait-command that is used to wait a certain amount of seconds implemented in the host language C#
/// </summary>
internal class WaitFunction : IFunction
{
    public int Arity => 1;

    public object? Call(List<object?> arguments)
    {
        if (arguments[0] is double seconds)
        {
            if(seconds >= 0)
            {
                Thread.Sleep((int)(1000 * seconds));
            }
        }
        return null;
    }

    public override string ToString() => "native wait function";
}
