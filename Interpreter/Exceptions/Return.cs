namespace Interpreter.Exceptions
{
    /// <summary>
    /// Return Exception used for control flow
    /// </summary>
    internal class Return : ApplicationException
    {
        /// <summary>
        /// The value that is returned
        /// </summary>
        public object? Value { get; init; }

        public Return(object? Value)
        {
            this.Value = Value;
        }
    }
}
