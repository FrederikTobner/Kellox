namespace Interpreter.Expressions
{
    /// <summary>
    /// Models an assignment expression
    /// </summary>
    internal class AssignmentExpression : IExpression
    {
        /// <summary>
        /// The Token of the Assignment expression (the variable name/identifier)
        /// </summary>
        public Token Token { get; init; }

        /// <summary>
        /// The Token of the Assignement expression (the value associated with the variable identifier)
        /// </summary>
        public IExpression Value { get; init; }

        public AssignmentExpression(Token token, IExpression value)
        {
            this.Token = token;
            this.Value = value;
        }

        /// <summary>
        /// Returns a representation of the Expression as a string
        /// </summary>
        public override string ToString() => IExpression.Parenthesize(Token.Lexeme, Value);

        public object? EvaluateExpression()
        {
            object? result = Value.EvaluateExpression();
            CustomInterpreter.currentEnvironment.Assign(Token, result);
            return result;
        }
    }
}
