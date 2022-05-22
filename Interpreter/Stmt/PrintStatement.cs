namespace Interpreter.Stmt
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
            Console.WriteLine(expression.EvaluateExpression());
        }
    }
}
