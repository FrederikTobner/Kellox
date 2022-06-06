namespace Interpreter.Utils
{
    /// <summary>
    /// Stores all the keywords of the language lox
    /// </summary>
    internal static class LoxKeywords
    {
        /// <summary>
        /// /The Dictionary containing the keywords
        /// </summary>
        private static readonly Dictionary<string, TOKENTYPE> keywords = InitializeKeywords();

        /// <summary>
        /// Gets the type of a given Token
        /// </summary>
        internal static bool GetTokenType(string key, out TOKENTYPE type) => keywords.TryGetValue(key, out type);

        /// <summary>
        /// Initializes the Dictionary with the Keywords of the language
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, TOKENTYPE> InitializeKeywords()
        {
            Dictionary<string, TOKENTYPE> words = new();
            words.Add("and", TOKENTYPE.AND);
            words.Add("class", TOKENTYPE.CLASS);
            words.Add("else", TOKENTYPE.ELSE);
            words.Add("false", TOKENTYPE.FALSE);
            words.Add("for", TOKENTYPE.FOR);
            words.Add("fun", TOKENTYPE.FUN);
            words.Add("if", TOKENTYPE.IF);
            words.Add("nil", TOKENTYPE.NIL);
            words.Add("or", TOKENTYPE.OR);
            words.Add("print", TOKENTYPE.PRINT);
            words.Add("return", TOKENTYPE.RETURN);
            words.Add("super", TOKENTYPE.SUPER);
            words.Add("this", TOKENTYPE.THIS);
            words.Add("true", TOKENTYPE.TRUE);
            words.Add("var", TOKENTYPE.VAR);
            words.Add("while", TOKENTYPE.WHILE);
            return words;
        }
    }
}
