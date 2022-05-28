namespace Interpreter.Statements
{
    /// <summary>
    /// Models a print statement
    /// </summary>
    internal class PrintStatement : IStatement
    {
        /// <summary>
        /// The Expression that shall be printed e.g "Hallo" / 6.3
        /// </summary>
        public IExpression Expression { get; init; }

        public PrintStatement(IExpression expression)
        {
            this.Expression = expression;
        }

        public void ExecuteStatement()
        {
            object? obj = Expression.EvaluateExpression();
            if (obj is not null)
            {
                Console.WriteLine(obj);
            }
            else
            {
                Console.WriteLine("nil");
            }
        }
    }
}
