namespace Interpreter.Exceptions
{
    /// <summary>
    /// Represents an Error that has occured during the parsing process
    /// </summary>
    internal class ParseError : ApplicationException
    {
        public Token ErrorToken { get; init; }

        public ParseError(Token ErrorToken, string Message) : base(Message)
        {
            this.ErrorToken = ErrorToken;
        }
    }

}
