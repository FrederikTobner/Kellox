
namespace Interpreter
{
    internal class Scanner
    {
        private readonly string source;
        private readonly List<Token> tokens = new();
        private int start = 0;
        private int current = 0;
        private int line = 1;

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
            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void AddToken(TokenType tokenType) => AddToken(tokenType, null);

        private void AddToken(TokenType tokenType, Object? literal)
        {
            string text = source[start..current];
            tokens.Add(new Token(tokenType, text, literal, line));
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

        private static readonly Dictionary<string, TokenType> keywords = InitializeKeywords();

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

        private static bool IsAlpha(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c is '_';

        private static bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            string text = source[start..current];
            if (!keywords.TryGetValue(text, out TokenType type))
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
            string value = source.Substring(start + 1, current - start - 1);
            AddToken(TokenType.STRING, value);
        }

        private static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

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
            AddToken(TokenType.NUMBER, Convert.ToDouble(source[start..current]));
        }

        private char PeekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }

        private char Peek()
        {
            if (IsAtEnd())
            {
                return '\0';
            }
            return source[current];
        }

        private bool Match(char expected)
        {
            if (IsAtEnd())
            {
                return false;
            }
            if (source[current] != expected)
            {
                return false;
            }

            current++;
            return true;
        }

        private char Advance() => source[current++];

        private bool IsAtEnd() => current >= source.Length;
    }
}

