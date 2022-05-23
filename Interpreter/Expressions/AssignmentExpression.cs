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

        public override string ToString() => IExpression.Parenthesize(token.Lexeme, value);

        public object? EvaluateExpression()
        {
            object? result = value.EvaluateExpression();
            CustomInterpreter.customEnvironment.Assign(token, result);
            return result;
        }
    }
}
