using Kellox.Exceptions;
using Kellox.Tokens;

namespace Kellox.Functions;

/// <summary>
/// Native Function Read implemented in the host language C#
/// </summary>
internal class ReadFunction : IFunction
{
    public int Arity => 0;

    public object? Call(List<object?> arguments, Token paren)
    {
        try
        {
        string? text = Console.ReadLine();
        return text is not null ? text : string.Empty;
        }
        catch(IOException)
        {
            throw new RunTimeError(paren, "An I/O error occurred");
        }
        catch(OutOfMemoryException)
        {
            throw new RunTimeError(paren, "There is insufficient memory to allocate a buffer for the returned string.");
        }
        catch(ArgumentOutOfRangeException)
        {
            throw new RunTimeError(paren, "The number of characters in the next line of characters is greater than System.Int32.MaxValue.");
        }
    }

    public override string ToString() => "native read function";
}
