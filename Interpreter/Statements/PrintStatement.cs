namespace Interpreter.Statements
{
    internal class PrintStatement : IStatement
    {
        readonly IExpression expression;

        public PrintStatement(IExpression expression)
        {
            this.expression = expression;
        }
        public void ExecuteStatement()
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
