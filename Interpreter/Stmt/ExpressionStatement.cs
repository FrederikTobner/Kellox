namespace Interpreter.Stmt
{
    internal class ExpressionStatement : IStatement
    {
        IExpression expression;

        public ExpressionStatement(IExpression expression)
        {
            this.expression = expression;
        }

        public void ExecuteStatement()
        {
            expression.EvaluateExpression();
        }
    }
}
