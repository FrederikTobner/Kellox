using static Interpreter.TokenType;

namespace Interpreter
{
    internal class Scanner
    {
        private readonly string source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private readonly int line = 1;

        public Scanner(string source)
        {
            this.source = source;
        }

        internal List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }
            tokens.Add(new Token(EOF, "", null, line));
            return tokens;
        }

        private void AddToken(TokenType tokenType) => AddToken(tokenType, null);

        private void AddToken(TokenType tokenType, Object? literal)
        {
            string text = source.Substring(start, current);
            tokens.Add(new Token(tokenType, text, literal, line));
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(LEFT_PAREN); break;
                case ')': AddToken(RIGHT_PAREN); break;
                case '{': AddToken(LEFT_BRACE); break;
                case '}': AddToken(RIGHT_BRACE); break;
                case ',': AddToken(COMMA); break;
                case '.': AddToken(DOT); break;
                case '-': AddToken(MINUS); break;
                case '+': AddToken(PLUS); break;
                case ';': AddToken(SEMICOLON); break;
                case '*': AddToken(STAR); break;
            }
        }

        private char Advance() => source[current++];

        private bool IsAtEnd() => current >= source.Length;
    }
}

