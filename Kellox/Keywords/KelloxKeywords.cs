using Kellox.Tokens;

namespace Kellox.Keywords
{
    /// <summary>
    /// Stores all the keywords of the language lox
    /// </summary>
    internal static class KelloxKeywords
    {
        /// <summary>
        /// The Dictionary containing the keywords
        /// </summary>
        private static readonly Dictionary<string, TokenType> keywords = InitializeKeywords();

        /// <summary>
        /// Gets the type of a given Token
        /// </summary>
        internal static bool TryGetKeywordTokenType(string key, out TokenType type) => keywords.TryGetValue(key, out type);

        /// <summary>
        /// Initializes the Dictionary with the Keywords of the language
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, TokenType> InitializeKeywords()
        {
            Dictionary<string, TokenType> words = new();
            words.Add(KeywordConstants.AndKeyword, TokenType.AND);
            words.Add(KeywordConstants.ClassKeyword, TokenType.CLASS);
            words.Add(KeywordConstants.ElseKeyword, TokenType.ELSE);
            words.Add(KeywordConstants.FalseKeyword, TokenType.FALSE);
            words.Add(KeywordConstants.ForKeyWord, TokenType.FOR);
            words.Add(KeywordConstants.FunctionKeyword, TokenType.FUN);
            words.Add(KeywordConstants.IfKeyword, TokenType.IF);
            words.Add(KeywordConstants.NilKeyword, TokenType.NIL);
            words.Add(KeywordConstants.OrKeyword, TokenType.OR);
            words.Add(KeywordConstants.PrintKeyword, TokenType.PRINT);
            words.Add(KeywordConstants.PrintLineKeyword, TokenType.PRINTLN);
            words.Add(KeywordConstants.ReturnKeyword, TokenType.RETURN);
            words.Add(KeywordConstants.SuperKeyword, TokenType.SUPER);
            words.Add(KeywordConstants.ThisKeyword, TokenType.THIS);
            words.Add(KeywordConstants.TrueKeyword, TokenType.TRUE);
            words.Add(KeywordConstants.VarKeyword, TokenType.VAR);
            words.Add(KeywordConstants.WhileKeyword, TokenType.WHILE);
            return words;
        }
    }
}
