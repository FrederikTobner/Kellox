namespace Interpreter.Expr
{
    internal class VariableExpression : IExpression
    {
        private readonly Token token;

        public VariableExpression(Token token)
        {
            this.token = token;
        }

        internal Token Token => token;

        public object? EvaluateExpression() => CustomInterpreter.customEnvironment.Get(token);

        public override string ToString() => $"Var ({token.Lexeme}) ";
    }
}
