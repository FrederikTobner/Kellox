using Interpreter.Expressions;

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
        public Token Name { get; init; }

        /// <summary>
        /// The expression that evaluated and assigned to the variable
        /// </summary>
        public IExpression? Expression { get; init; }

        public DeclarationStatement(Token name, IExpression? expression)
        {
            this.Name = name;
            this.Expression = expression;
        }

        public void ExecuteStatement()
        {
            object? value = null;
            if (Expression is not null)
            {
                value = Expression.EvaluateExpression();
            }
            LoxInterpreter.currentEnvironment.Define(Name.Lexeme, value);
        }
    }
}
