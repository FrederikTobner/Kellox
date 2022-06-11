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

        /// <summary>
        /// Constructor of the RunTimeError exception
        /// </summary>
        /// <param name="token">The Token that has triggered the runtime error</param>
        /// <param name="message">The message of the Exception</param>
        public RunTimeError(Token token, string message) : base(message)
        {
            this.Token = token;
        }
    }
}
