namespace Interpreter.Statements
{
    /// <summary>
    /// Models a print statement
    /// </summary>
    internal class PrintStatement : IStatement
    {
        readonly IExpression expression;

        public PrintStatement(IExpression expression)
        {
            this.expression = expression;
        }

        public void ExecuteStatements()
        {
            object? obj = expression.EvaluateExpression();
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
