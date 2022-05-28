namespace Interpreter.Expressions
{
    internal class VariableExpression : IExpression
    {
        public Token Token { get; init; }

        public VariableExpression(Token token)
        {
            this.Token = token;
        }

        public object? EvaluateExpression() => CustomInterpreter.currentEnvironment.Get(Token);

        /// <summary>
        /// Returns a representation of the Expression as a string
        /// </summary>
        public override string ToString() => $"Var ({Token.Lexeme}) ";
    }
}
