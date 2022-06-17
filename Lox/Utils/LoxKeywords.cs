using Lox.Messages;
using Lox.Tokens;

namespace Lox.Utils
{
    /// <summary>
    /// Stores all the keywords of the language lox
    /// </summary>
    internal static class LoxKeywords
    {
        /// <summary>
        /// /The Dictionary containing the keywords
        /// </summary>
        private static readonly Dictionary<string, TokenType> keywords = InitializeKeywords();

        /// <summary>
        /// Gets the type of a given Token
        /// </summary>
        internal static bool GetTokenType(string key, out TokenType type) => keywords.TryGetValue(key, out type);

        /// <summary>
        /// Initializes the Dictionary with the Keywords of the language
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, TokenType> InitializeKeywords()
        {
            Dictionary<string, TokenType> words = new();
            words.Add(MessageUtils.AndKeyword, TokenType.AND);
            words.Add(MessageUtils.ClassKeyword, TokenType.CLASS);
            words.Add(MessageUtils.ElseKeyword, TokenType.ELSE);
            words.Add(MessageUtils.FalseKeyword, TokenType.FALSE);
            words.Add(MessageUtils.ForKeyWord, TokenType.FOR);
            words.Add(MessageUtils.FunctionKeyword, TokenType.FUN);
            words.Add(MessageUtils.IfKeyword, TokenType.IF);
            words.Add(MessageUtils.NilKeyword, TokenType.NIL);
            words.Add(MessageUtils.OrKeyword, TokenType.OR);
            words.Add(MessageUtils.PrintKeyword, TokenType.PRINT);
            words.Add(MessageUtils.PrintLineKeyword, TokenType.PRINTLN);
            words.Add(MessageUtils.ReturnKeyword, TokenType.RETURN);
            words.Add(MessageUtils.SuperKeyword, TokenType.SUPER);
            words.Add(MessageUtils.ThisKeyword, TokenType.THIS);
            words.Add(MessageUtils.TrueKeyword, TokenType.TRUE);
            words.Add(MessageUtils.VarKeyword, TokenType.VAR);
            words.Add(MessageUtils.WhileKeyword, TokenType.WHILE);
            return words;
        }
    }
}
