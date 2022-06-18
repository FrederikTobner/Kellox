using Kellox.i18n;
using Kellox.Tokens;

namespace Kellox.Utils
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
            words.Add(Constants.AndKeyword, TokenType.AND);
            words.Add(Constants.ClassKeyword, TokenType.CLASS);
            words.Add(Constants.ElseKeyword, TokenType.ELSE);
            words.Add(Constants.FalseKeyword, TokenType.FALSE);
            words.Add(Constants.ForKeyWord, TokenType.FOR);
            words.Add(Constants.FunctionKeyword, TokenType.FUN);
            words.Add(Constants.IfKeyword, TokenType.IF);
            words.Add(Constants.NilKeyword, TokenType.NIL);
            words.Add(Constants.OrKeyword, TokenType.OR);
            words.Add(Constants.PrintKeyword, TokenType.PRINT);
            words.Add(Constants.PrintLineKeyword, TokenType.PRINTLN);
            words.Add(Constants.ReturnKeyword, TokenType.RETURN);
            words.Add(Constants.SuperKeyword, TokenType.SUPER);
            words.Add(Constants.ThisKeyword, TokenType.THIS);
            words.Add(Constants.TrueKeyword, TokenType.TRUE);
            words.Add(Constants.VarKeyword, TokenType.VAR);
            words.Add(Constants.WhileKeyword, TokenType.WHILE);
            return words;
        }
    }
}
