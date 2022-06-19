using Kellox.Interpreter;
using Kellox.Tokens;

namespace Kellox.Expressions;

/// <summary>
/// Models a This expressionn in lox
/// </summary>
internal class ThisExpression : IExpression
{
    /// <summary>
    /// The Token of the this expression
    /// </summary>
    public Token Token { get; init; }

    public ThisExpression(Token Keyword)
    {
        this.Token = Keyword;
    }

    public object? Evaluate()
    {
        if (KelloxInterpreter.TryGetDepthOfLocal(this, out int distance))
        {
            return KelloxInterpreter.currentEnvironment.GetAt(distance, Token);
        }
        else
        {
            return KelloxInterpreter.currentEnvironment.Get(Token);
        }
    }

    public override string ToString() => $"{Token.Lexeme} -> {this.Evaluate()}.";
}
