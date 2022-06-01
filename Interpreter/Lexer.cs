
namespace Interpreter
{
    /// <summary>
    /// Performs a lexical analysis on a linear stream of characters and groups them together to a flat sequence of Tokens
    /// </summary>
    internal class Lexer
    {
        /// <summary>
        /// StartIndex of the token that is currently getting evaluated in the sourcecode
        /// </summary>
        private int start = 0;

        /// <summary>
        /// CurrentPosition in the sourcecode
        /// </summary>
        private int current = 0;

        /// <summary>
        /// The current line in the sourcecode
        /// </summary>
        private int line = 1;

        /// <summary>
        /// The sourceCode
        /// </summary>
        public string Source { get; init; }

        /// <summary>
        /// The Tokens that make up the sourceCode
        /// </summary>
        public List<Token> Tokens { get; init; }

        public Lexer(string source)
        {
            this.Source = source;
            this.Tokens = new List<Token>();
        }

        /// <summary>
        /// Scans the Tokens in a file and returns the, as a List
        /// </summary>
        internal List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }
            //Adding the end of file Token
            Tokens.Add(new Token(TokenType.EOF, "", null, line));
            return Tokens;
        }

        /// <summary>
        /// Adds a single Token
        /// </summary>
        private void AddToken(TokenType tokenType) => AddToken(tokenType, null);

        /// <summary>
        /// Adds a single LiteralToken
        /// </summary>       
        private void AddToken(TokenType tokenType, object? literal)
        {
            string text = Source[start..current];
            Tokens.Add(new Token(tokenType, text, literal, line));
        }

        /// <summary>
        /// Scans the next Token
        /// </summary>
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
                    break;
                case '\n':
                    line++;
                    break;
                case '"':
                    AString();
                    break;
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
                        Program.Error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        /// <summary>
        /// Determines weather a char is from the Alphabet
        /// </summary>
        /// <param name="c">The char that is evaluated</param>
        private static bool IsAlpha(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c is '_';

        /// <summary>
        /// Determines weather a char is from the Alphabet or a number
        /// </summary>
        /// <param name="c">The char that is evaluated</param>
        private static bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);

        /// <summary>
        /// Indicates weather the Lexer has reached the end of the file
        /// </summary>
        private bool IsAtEnd() => current >= Source.Length;

        /// <summary>
        /// Used to create a Token with the Tokentype identifier (meaning the name of a variable)
        /// </summary>
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

        /// <summary>
        /// Used to create a Token with the Tokentype String (a string literal)
        /// </summary>
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
                Program.Error(line, "Unterminated string.");
                return;
            }
            Advance();

            // Trims the surrounding quotes.
            string value = Source.Substring(start + 1, current - start - 2);
            AddToken(TokenType.STRING, value);
        }

        /// <summary>
        /// Determines weather a char is a number (0-9)
        /// </summary>
        /// <param name="c">The char that is evaluated</param>
        private static bool IsDigit(char c) => c >= '0' && c <= '9';

        /// <summary>
        /// Used to create a Token with the Tokentype Number (a number literal)
        /// </summary>
        private void Number()
        {
            int digitsAfterPoint = 0;

            while (IsDigit(Peek()))
            {
                Advance();
            }

            // Look for a fractional part.
            if (Peek() is '.' && IsDigit(PeekNext()))
            {
                // Consumes the "."
                Advance();
                while (IsDigit(Peek()))
                {
                    Advance();
                    digitsAfterPoint++;
                }
            }
            double number = Convert.ToDouble(Source[start..current]) / Math.Pow(10, digitsAfterPoint);
            AddToken(TokenType.NUMBER, number);
        }

        /// <summary>
        /// ´Returns the char one position ahead of the current Position
        /// </summary>
        private char PeekNext()
        {
            if (current + 1 >= Source.Length)
            {
                return '\0';
            }
            return Source[current + 1];
        }

        /// <summary>
        /// Returns the char at the current position
        /// </summary>
        private char Peek()
        {
            if (IsAtEnd())
            {
                return '\0';
            }
            return Source[current];
        }

        /// <summary>
        /// Matches the char of the current position with the passed char
        /// </summary>
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

        /// <summary>
        /// Advances a postion further in the sourceCode (one character)
        /// </summary>
        private char Advance() => Source[current++];
    }
}

