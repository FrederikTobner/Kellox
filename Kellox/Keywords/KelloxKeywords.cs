using Kellox.Tokens;

namespace Kellox.Keywords
{
    /// <summary>
    /// Stores all the keywords of the language Kellox
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
        private static Dictionary<string, TokenType> InitializeKeywords()
        {
            Dictionary<string, TokenType> keyWords = new();
            keyWords.Add(KeywordConstants.AndKeyword, TokenType.AND);
            keyWords.Add(KeywordConstants.BreakKeyword, TokenType.BREAK);
            keyWords.Add(KeywordConstants.ClassKeyword, TokenType.CLASS);
            keyWords.Add(KeywordConstants.ElseKeyword, TokenType.ELSE);
            keyWords.Add(KeywordConstants.FalseKeyword, TokenType.FALSE);
            keyWords.Add(KeywordConstants.ForKeyWord, TokenType.FOR);
            keyWords.Add(KeywordConstants.FunctionKeyword, TokenType.FUN);
            keyWords.Add(KeywordConstants.IfKeyword, TokenType.IF);
            keyWords.Add(KeywordConstants.NilKeyword, TokenType.NIL);
            keyWords.Add(KeywordConstants.OrKeyword, TokenType.OR);
            keyWords.Add(KeywordConstants.PrintKeyword, TokenType.PRINT);
            keyWords.Add(KeywordConstants.PrintLineKeyword, TokenType.PRINTLN);
            keyWords.Add(KeywordConstants.ReturnKeyword, TokenType.RETURN);
            keyWords.Add(KeywordConstants.SuperKeyword, TokenType.SUPER);
            keyWords.Add(KeywordConstants.ThisKeyword, TokenType.THIS);
            keyWords.Add(KeywordConstants.TrueKeyword, TokenType.TRUE);
            keyWords.Add(KeywordConstants.VarKeyword, TokenType.VAR);
            keyWords.Add(KeywordConstants.WhileKeyword, TokenType.WHILE);
            return keyWords;
        }
    }
}
