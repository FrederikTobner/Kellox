namespace Interpreter.Statements
{
    /// <summary>
    /// Models a declaration statement
    /// </summary>
    internal class DeclarationStatement : IStatement
    {
        /// <summary>
        /// The Identifier Token of this declaration statement
        /// </summary>
        private readonly Token name;

        /// <summary>
        /// The expression that evaluated and assigned to the variable
        /// </summary>
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
