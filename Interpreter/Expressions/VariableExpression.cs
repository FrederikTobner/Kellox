namespace Interpreter.Expressions
{
    internal class VariableExpression : IExpression
    {
        private readonly Token token;

        public VariableExpression(Token token)
        {
            this.token = token;
        }

        internal Token Token => token;

        public object? EvaluateExpression() => CustomInterpreter.currentEnvironment.Get(token);

        /// <summary>
        /// Returns a representation of the Expression as a string
        /// </summary>
        public override string ToString() => $"Var ({token.Lexeme}) ";
    }
}
