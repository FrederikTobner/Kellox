namespace Interpreter.Expressions
{
    /// <summary>
    /// Models a Variable expression in lox
    /// </summary>
    internal class VariableExpression : IExpression
    {
        /// <summary>
        /// The Token of the Variable expression (the variable name/identifier)
        /// </summary>
        public Token Token { get; init; }

        public VariableExpression(Token token)
        {
            this.Token = token;
        }

        public object? EvaluateExpression()
        {
            if (LoxInterpreter.locals.TryGetValue(this, out int distance))
            {
                return LoxInterpreter.currentEnvironment.GetAt(distance, Token);
            }
            else
            {
                return LoxInterpreter.globalEnvironment.Get(Token);
            }
        }

        /// <summary>
        /// Returns a representation of the Expression as a string
        /// </summary>
        public override string ToString() => $"Var ({Token.Lexeme}) ";
    }
}
