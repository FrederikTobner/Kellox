namespace Interpreter.Expressions
{
    /// <summary>
    /// Models a This expressionn in lox
    /// </summary>
    internal class ThisExpression : IExpression
    {
        public Token Keyword { get; init; }

        public ThisExpression(Token Keyword)
        {
            this.Keyword = Keyword;
        }

        public object? EvaluateExpression()
        {
            if (LoxInterpreter.locals.TryGetValue(this, out int distance))
            {
                return LoxInterpreter.currentEnvironment.GetAt(distance, Keyword);
            }
            else
            {
                return LoxInterpreter.currentEnvironment.Get(Keyword);
            }
        }

        public override string ToString() => "this -> " + this.EvaluateExpression() + ".";
    }
}
