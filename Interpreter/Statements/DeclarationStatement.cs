namespace Interpreter.Statements
{
    internal class DeclarationStatement : IStatement
    {
        private readonly Token name;
        private readonly IExpression expression;

        public DeclarationStatement(Token name, IExpression expression)
        {
            this.name = name;
            this.expression = expression;
        }

        public void ExecuteStatements()
        {
            object? value = null;
            if (expression != null)
            {
                value = expression.EvaluateExpression();
            }
            CustomInterpreter.currentEnvironment.Define(name.Lexeme, value);
        }
    }
}
