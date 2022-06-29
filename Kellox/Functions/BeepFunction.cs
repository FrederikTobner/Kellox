using Kellox.Exceptions;
using Kellox.Tokens;
using System.Runtime.Versioning;

namespace Kellox.Functions;

/// <summary>
/// Native Function Beep implemented in the host language C#
/// </summary>
internal class BeepFunction : IFunction
{
    public int Arity => 2;

    [SupportedOSPlatform("windows")]
    public object? Call(List<object?> arguments, Token paren)
    {
        if (arguments[0] is not double frequency || arguments[1] is not double duration)
        {
            throw new RunTimeError(paren, "Can't call beep function with anything that is not a Number");
        }
        if ((frequency - Math.Truncate(frequency)) >= double.Epsilon || (duration - Math.Truncate(duration)) >= double.Epsilon)
        {
            throw new RunTimeError(paren, "Arguments for beepCall call are not only whole number ");
        }
        try
        {
            // Plays beep sound 🎵
            Console.Beep((int)frequency, (int)duration);
            return null;
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new RunTimeError(paren, "frequency is less than 37 or more than 32767 hertz. -or- duration is less than or equal to zero.");
        }
        catch (PlatformNotSupportedException)
        {
            throw new RunTimeError(paren, "The beep function is only supported on windows");
        }
    }

    public override string ToString() => "native beep function";
}
