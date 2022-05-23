namespace Interpreter.Statements
{
    internal class ExpressionStatement : IStatement
    {
        readonly IExpression expression;

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
