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
            words.Add("and", TokenType.AND);
            words.Add("class", TokenType.CLASS);
            words.Add("else", TokenType.ELSE);
            words.Add("false", TokenType.FALSE);
            words.Add("for", TokenType.FOR);
            words.Add("fun", TokenType.FUN);
            words.Add("if", TokenType.IF);
            words.Add("nil", TokenType.NIL);
            words.Add("or", TokenType.OR);
            words.Add("print", TokenType.PRINT);
            words.Add("return", TokenType.RETURN);
            words.Add("super", TokenType.SUPER);
            words.Add("this", TokenType.THIS);
            words.Add("true", TokenType.TRUE);
            words.Add("var", TokenType.VAR);
            words.Add("while", TokenType.WHILE);
            return words;
        }
    }
}
