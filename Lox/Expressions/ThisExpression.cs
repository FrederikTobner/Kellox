using Lox.Interpreter;
using Lox.LexicalAnalysis;

namespace Lox.Expressions;

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

    public object? EvaluateExpression()
    {
        if (LoxInterpreter.TryGetDepthOfLocal(this, out int distance))
        {
            return LoxInterpreter.currentEnvironment.GetAt(distance, Token);
        }
        else
        {
            return LoxInterpreter.currentEnvironment.Get(Token);
        }
    }

    public override string ToString() => $"{Token.Lexeme} -> {this.EvaluateExpression()}.";
}
