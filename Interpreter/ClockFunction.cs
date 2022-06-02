namespace Interpreter
{
    /// <summary>
    /// Native Function Clock implemented in the host language C#
    /// </summary>
    internal class ClockFunction : ILoxCallable
    {
        public int Arity => 0;

        public object? Call(List<object?> arguments) => DateTime.Now.Second;

        public override string ToString() => "native clock function";
    }
}
