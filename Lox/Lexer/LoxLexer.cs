using Lox.Tokens;
using Lox.Utils;

namespace Lox.Lexer;

/// <summary>
/// Performs a lexical analysis on a linear stream of characters and groups them together to a flat sequence of Tokens
/// </summary>
internal class LoxLexer
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

    /// <summary>
    /// Constructor of the Lexer
    /// </summary>
    /// <param name="source">The flat sequence of tokens that is grouped to a flat sequence of Tokens by the Lexer</param>
    public LoxLexer(string source)
    {
        Source = source;
        Tokens = new List<Token>();
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
        Tokens.Add(new Token(TokenType.EOF, string.Empty, null, line));
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
                AddToken(TokenType.LEFT_PARENTHESIS);
                break;
            case ')':
                AddToken(TokenType.RIGHT_PARENTHESIS);
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
                    break;
                }
                Identifier();
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
                if (ScanUtils.IsDigit(c))
                {
                    Number();
                }
                else if (ScanUtils.IsAlpha(c))
                {
                    Identifier();
                }
                else
                {
                    LoxErrorLogger.Error(line, "Unexpected character.");
                }
                break;
        }
    }

    /// <summary>
    /// Indicates weather the Lexer has reached the end of the file
    /// </summary>
    private bool IsAtEnd() => current >= Source.Length;

    /// <summary>
    /// Used to create a Token with the Tokentype identifier (meaning the name of a variable)
    /// </summary>
    private void Identifier()
    {
        while (ScanUtils.IsAlphaNumeric(Peek()))
        {
            Advance();
        }

        string text = Source[start..current];
        if (!LoxKeywords.GetTokenType(text, out TokenType type))
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
            LoxErrorLogger.Error(line, "Unterminated string.");
            return;
        }
        Advance();

        // Trims the surrounding quotes.
        string value = Source.Substring(start + 1, current - start - 2);
        AddToken(TokenType.STRING, value);
    }

    /// <summary>
    /// Used to create a Token with the Tokentype Number (a number literal)
    /// </summary>
    private void Number()
    {
        int digitsAfterPoint = 0;

        while (ScanUtils.IsDigit(Peek()))
        {
            Advance();
        }

        // Look for a fractional part.
        if (Peek() is '.' && ScanUtils.IsDigit(PeekNext()))
        {
            // Consumes the "."
            Advance();
            while (ScanUtils.IsDigit(Peek()))
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
    /// Advances a postion further in the sourceCode (one character) and returns the character at the previous position
    /// </summary>
    private char Advance() => Source[current++];
}

