namespace Interpreter.Stmt
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

        public void ExecuteStatement()
        {
            object? value = null;
            if (expression != null)
            {
                value = expression.EvaluateExpression();
            }
            CustomInterpreter.customEnvironment.Define(name.Lexeme, value);
        }
    }
}
