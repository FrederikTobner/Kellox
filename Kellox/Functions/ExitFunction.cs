using System.Security;
using Kellox.Exceptions;
using Kellox.Tokens;

namespace Kellox.Functions;

internal class ExitFunction : IFunction
{
    public int Arity => 1;

    public object? Call(List<object?> arguments, Token paren)
    {
        if (arguments[0] is not double exitCode)
        {
            throw new RunTimeError(paren, "Arguments for exitCall call is not a number ");
        }
        if ((exitCode - Math.Truncate(exitCode)) >= double.Epsilon)
        {
            throw new RunTimeError(paren, "Arguments for exitCall call is not a whole number ");
        }
        try
        {
            Environment.Exit((int)exitCode);
            return null;
        }
        catch (SecurityException)
        {
            throw new RunTimeError(paren, "You do not have sufficient security permission to perform this function.");
        }

    }

    public override string ToString() => "native exit function";
}