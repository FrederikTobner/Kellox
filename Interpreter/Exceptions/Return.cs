namespace Interpreter.Exceptions
{
    internal class Return : ApplicationException
    {
        public object? Value { get; init; }

        public Return(object? Value)
        {
            this.Value = Value;
        }
    }
}
