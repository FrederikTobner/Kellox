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
        private readonly Token token;

        public RunTimeError(Token token, string message) : base(message)
        {
            this.token = token;
        }

        /// <summary>
        /// The Token that has triggered the runtime error
        /// </summary>
        internal Token Token => token;
    }
}
