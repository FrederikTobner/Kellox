using Kellox.Interpreter;
using Kellox.Tokens;

namespace Kellox.Expressions;

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
    public override string ToString() => $"{Token.Lexeme} = {Value}";

    public object? Evaluate()
    {
        object? result = Value.Evaluate();
        if (KelloxInterpreter.TryGetDepthOfLocal(this, out int distance))
        {
            KelloxInterpreter.currentEnvironment.AssignAt(distance, Token, result);
            return result;
        }
        KelloxInterpreter.globalEnvironment.Assign(Token, result);
        return result;
    }
}
