using Kellox.Exceptions;
using Kellox.Tokens;

namespace Kellox.Statements;

internal class BreakStatement : IStatement
{

    public Token Token { get; init; }

    public BreakStatement(Token breakToken)
    {
        this.Token = breakToken;
    }

    public void Execute()
    {
        throw new Break();
    }
}
