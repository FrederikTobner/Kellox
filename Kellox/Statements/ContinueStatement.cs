using Kellox.Exceptions;
using Kellox.Tokens;

namespace Kellox.Statements
{
    internal class ContinueStatement : IStatement
    {

        public Token Token { get; init; }

        public ContinueStatement(Token continueToken)
        {
            this.Token = continueToken;
        }

        public void Execute()
        {
            throw new Continue();
        }
    }
}