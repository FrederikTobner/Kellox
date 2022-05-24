namespace Interpreter.Expressions
{
    internal class AssignmentExpression : IExpression
    {
        readonly Token token;

        readonly IExpression value;

        public AssignmentExpression(Token token, IExpression value)
        {
            this.token = token;
            this.value = value;
        }

        /// <summary>
        /// Returns a representation of the Expression as a string
        /// </summary>
        public override string ToString() => IExpression.Parenthesize(token.Lexeme, value);

        public object? EvaluateExpression()
        {
            object? result = value.EvaluateExpression();
            CustomInterpreter.currentEnvironment.Assign(token, result);
            return result;
        }
    }
}
