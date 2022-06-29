namespace Kellox.Exceptions;

internal class ArgumentError : ApplicationException
{
    public ArgumentError(string? message) : base(message)
    {
    }
}
