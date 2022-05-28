namespace Interpreter.Statements
{
    /// <summary>
    /// Models a epression statement
    /// </summary>
    internal class ExpressionStatement : IStatement
    {
        readonly IExpression expression;

        public ExpressionStatement(IExpression expression)
        {
            this.expression = expression;
        }

        public void ExecuteStatements()
        {
            expression.EvaluateExpression();
        }
    }
}
