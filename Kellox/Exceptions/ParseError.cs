using Kellox.Tokens;

namespace Kellox.Exceptions;

/// <summary>
/// Represents an Error that has occured during the parsing process
/// </summary>
internal class ParseError : ApplicationException
{
    /// <summary>
    /// The Token that has caused the Error during the parsing process
    /// </summary>
    public Token ErrorToken { get; init; }

    /// <summary>
    /// Constructor of the ParseError exception
    /// </summary>
    /// <param name="token">the Token that has triggered the runtime error</param>
    /// <param name="message">The message of the Exception</param>
    public ParseError(Token ErrorToken, string Message) : base(Message)
    {
        this.ErrorToken = ErrorToken;
    }
}

