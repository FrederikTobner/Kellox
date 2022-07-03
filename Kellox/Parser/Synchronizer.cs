using Kellox.Tokens;

namespace Kellox.Parser;

internal static class Synchronizer
{
    /// <summary>
    /// Can be used to parse statements after a statement that has caused a RunTimeError
    /// </summary>
    internal static void Synchronize(IReadOnlyList<Token> tokens, ref int current)
    {
        // Do we only need to ignore a statement or a whole block?
        int scopeDepth = 0;
        current++;
        SynchronizeWithTokenTypes(tokens, TokenType.SEMICOLON, ref current, ref scopeDepth);
    }

    private static void SynchronizeWithTokenTypes(IReadOnlyList<Token> tokens, TokenType tokenType, ref int current, ref int scopeDepth)
    {
        if (tokens[current].TokenType is TokenType.EOF)
        {
            return;
        }
        if (tokens[current - 1].TokenType == tokenType)
        {
            if (scopeDepth < 1)
            {
                return;
            }
            scopeDepth--;
        }
        if (tokens[current - 1].TokenType is TokenType.LEFT_BRACE)
        {
            scopeDepth++;
            if (tokenType is not TokenType.RIGHT_BRACE)
            {
                tokenType = TokenType.RIGHT_BRACE;
            }
        }
        current++;
        SynchronizeWithTokenTypes(tokens, tokenType, ref current, ref scopeDepth);
    }
}
