namespace Interpreter.Statements
{
    /// <summary>
    /// Models a print statement
    /// </summary>
    internal class PrintStatement : IStatement
    {
        public IExpression Expression { get; init; }

        public PrintStatement(IExpression expression)
        {
            this.Expression = expression;
        }

        public void ExecuteStatements()
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
