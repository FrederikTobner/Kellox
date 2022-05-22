namespace Interpreter.Stmt
{
    internal class PrintStatement : IStatement
    {
        IExpression expression;

        public PrintStatement(IExpression expression)
        {
            this.expression = expression;
        }
        public void ExecuteStatement()
        {
            Console.WriteLine(expression.EvaluateExpression());
        }
    }
}
