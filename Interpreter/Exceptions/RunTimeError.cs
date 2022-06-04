namespace Interpreter.Exceptions
{
    /// <summary>
    /// Models an Error that occured during runtime
    /// </summary>
    internal class RunTimeError : ApplicationException
    {
        /// <summary>
        /// The Token that has triggered the runtime error
        /// </summary>
        public Token Token { get; init; }

        public RunTimeError(Token token, string message) : base(message)
        {
            this.Token = token;
        }
    }
}
