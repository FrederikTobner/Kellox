using Kellox.Tokens;

namespace Kellox.Parser
{
    internal static class Synchronizer
    {
        /// <summary>
        /// Can be used to parse statements after a statement that has caused a RunTimeError
        /// </summary>
        internal static void Synchronize(IReadOnlyList<Token> tokens, ref int current)
        {
            // Do we only need to ignore a statement or the whole block?
            SynchronizeWithTokenTypes(tokens, tokens[current].TokenType is TokenType.LEFT_BRACE ? TokenType.RIGHT_BRACE : TokenType.SEMICOLON, ref current);
        }

        private static void SynchronizeWithTokenTypes(IReadOnlyList<Token> tokens, TokenType tokenType, ref int current)
        {
            if (tokens[current].TokenType is TokenType.EOF || tokens[current - 1].TokenType == tokenType)
            {
                return;
            }
            current++;
            SynchronizeWithTokenTypes(tokens, tokenType, ref current);
        }
    }
}
