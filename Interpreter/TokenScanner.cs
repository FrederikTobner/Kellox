
namespace Interpreter
{
    internal class TokenScanner
    {

        private int start = 0;
        private int current = 0;
        private int line = 1;

        public string Source { get; init; }

        public List<Token> Tokens { get; init; }

        public TokenScanner(string source)
        {
            this.Source = source;
            this.Tokens = new();
        }

        internal List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }
            Tokens.Add(new Token(TokenType.EOF, "", null, line));
            return Tokens;
        }

        private void AddToken(TokenType tokenType) => AddToken(tokenType, null);

        private void AddToken(TokenType tokenType, Object? literal)
        {
            string text = Source[start..current];
            Tokens.Add(new Token(tokenType, text, literal, line));
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(':
                    AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    AddToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    AddToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    AddToken(TokenType.RIGHT_BRACE);
                    break;
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case '.':
                    AddToken(TokenType.DOT);
                    break;
                case '-':
                    AddToken(TokenType.MINUS);
                    break;
                case '+':
                    AddToken(TokenType.PLUS);
                    break;
                case ';':
                    AddToken(TokenType.SEMICOLON);
                    break;
                case '*':
                    AddToken(TokenType.STAR);
                    break;
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                    if (Match('/'))
                    {
                        // A comment goes until the end of the line.
                        while (Peek() != '\n' && !IsAtEnd())
                        {
                            Advance();
                        }
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case 'o':
                    if (Match('r'))
                    {
                        AddToken(TokenType.OR);
                    }
                    break;
                case ' ':
                    break;
                case '\r':
                    break;
                case '\t':
                    // Ignore whitespace.
                    break;

                case '\n':
                    line++;
                    break;
                case '"': AString(); break;
                default:
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        CustomInterpreter.Error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        private static bool IsAlpha(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c is '_';

        private static bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);

        private bool IsAtEnd() => current >= Source.Length;

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            string text = Source[start..current];
            if (!KeywordsUtils.GetTokenType(text, out TokenType type))
            {
                type = TokenType.IDENTIFIER;
            }
            AddToken(type);
        }

        private void AString()
        {
            while (Peek() is not '\"' && !IsAtEnd())
            {
                if (Peek() is '\n')
                {
                    line++;
                }
                Advance();
            }

            if (IsAtEnd())
            {
                CustomInterpreter.Error(line, "Unterminated string.");
                return;
            }

            // The closing ".
            Advance();

            // Trim the surrounding quotes.
            string value = Source.Substring(start + 1, current - start - 2);
            AddToken(TokenType.STRING, value);
        }

        private static bool IsDigit(char c) => c >= '0' && c <= '9';

        private void Number()
        {
            while (IsDigit(Peek()))
            {
                Advance();
            }

            // Look for a fractional part.
            if (Peek() is '.' && IsDigit(PeekNext()))
            {
                // Consume the "."
                Advance();
                while (IsDigit(Peek()))
                {
                    Advance();
                }
            }
            AddToken(TokenType.NUMBER, Convert.ToDouble(Source[start..current]));
        }

        private char PeekNext()
        {
            if (current + 1 >= Source.Length)
            {
                return '\0';
            }
            return Source[current + 1];
        }

        private char Peek()
        {
            if (IsAtEnd())
            {
                return '\0';
            }
            return Source[current];
        }

        private bool Match(char expected)
        {
            if (IsAtEnd())
            {
                return false;
            }
            if (Source[current] != expected)
            {
                return false;
            }

            current++;
            return true;
        }

        private char Advance() => Source[current++];
    }
}

