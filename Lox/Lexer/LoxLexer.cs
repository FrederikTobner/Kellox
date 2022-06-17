using Lox.Tokens;
using Lox.Utils;

namespace Lox.Lexer;

/// <summary>
/// Performs a lexical analysis on a linear stream of characters and groups them together to a flat sequence of Tokens
/// </summary>
internal static class LoxLexer
{
    /// <summary>
    /// StartIndex of the token that is currently getting evaluated in the sourcecode
    /// </summary>
    private static int start = 0;

    /// <summary>
    /// CurrentPosition in the sourcecode
    /// </summary>
    private static int current = 0;

    /// <summary>
    /// The current line in the sourcecode
    /// </summary>
    private static int line = 1;

    /// <summary>
    /// The Tokens that make up the sourceCode
    /// </summary>
    private static readonly List<Token> tokens = new();

    /// <summary>
    /// Scans the Tokens in a file and returns the, as a List
    /// </summary>
    internal static List<Token> ScanTokens(string source)
    {
        start = 0;
        current = 0;
        line = 1;
        tokens.Clear();
        while (!IsAtEnd(source))
        {
            start = current;
            ScanToken(source);
        }
        //Adding the end of file Token
        tokens.Add(new Token(TokenType.EOF, string.Empty, null, line));
        return tokens;
    }

    /// <summary>
    /// Adds a single Token
    /// </summary>
    private static void AddToken(string source, TokenType tokenType) => AddToken(source, tokenType, null);

    /// <summary>
    /// Adds a single LiteralToken
    /// </summary>       
    private static void AddToken(string source, TokenType tokenType, object? literal)
    {
        string text = source[start..current];
        tokens.Add(new Token(tokenType, text, literal, line));
    }

    /// <summary>
    /// Scans the next Token
    /// </summary>
    private static void ScanToken(string source)
    {
        char c = Advance(source);
        switch (c)
        {
            case '(':
                AddToken(source, TokenType.LEFT_PARENTHESIS);
                break;
            case ')':
                AddToken(source, TokenType.RIGHT_PARENTHESIS);
                break;
            case '{':
                AddToken(source, TokenType.LEFT_BRACE);
                break;
            case '}':
                AddToken(source, TokenType.RIGHT_BRACE);
                break;
            case ',':
                AddToken(source, TokenType.COMMA);
                break;
            case '.':
                AddToken(source, TokenType.DOT);
                break;
            case '-':
                if (Match(source, '-'))
                {
                    AddToken(source, TokenType.MINUS_MINUS);
                }
                else
                {
                    AddToken(source, TokenType.MINUS);
                }
                break;
            case '+':
                if (Match(source, '+'))
                {
                    AddToken(source, TokenType.PLUS_PLUS);
                }
                else
                {
                    AddToken(source, TokenType.PLUS);
                }
                break;
            case ';':
                AddToken(source, TokenType.SEMICOLON);
                break;
            case '*':
                AddToken(source, TokenType.STAR);
                break;
            case '!':
                AddToken(source, Match(source, '=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;
            case '=':
                AddToken(source, Match(source, '=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '<':
                AddToken(source, Match(source, '=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '>':
                AddToken(source, Match(source, '=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
            case '/':
                if (Match(source, '/'))
                {
                    // A comment goes until the end of the line.
                    while (Peek(source) is not '\n' && !IsAtEnd(source))
                    {
                        Advance(source);
                    }
                }
                else if (Match(source, '*'))
                {
                    // A block comment goes until the next closing blockcomment tag
                    while (!Match(source, '*') && Peek(source) is not '/')
                    {
                        if (IsAtEnd(source))
                        {
                            //Blockcomment was never closed with a "*/"
                            LoxErrorLogger.Error(line, "Blockcomment was never closed, no \"*/\" found");
                            break;
                        }
                        Advance(source);
                    }
                    if (!IsAtEnd(source))
                        Advance(source);
                }
                else
                {
                    AddToken(source, TokenType.SLASH);
                }
                break;
            case 'o':
                if (Match(source, 'r'))
                {
                    AddToken(source, TokenType.OR);
                    break;
                }
                Identifier(source);
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
                AString(source);
                break;
            default:
                if (ScanUtils.IsDigit(c))
                {
                    Number(source);
                }
                else if (ScanUtils.IsAlpha(c))
                {
                    Identifier(source);
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
    private static bool IsAtEnd(string source) => current >= source.Length;

    /// <summary>
    /// Used to create a Token with the Tokentype identifier (meaning the name of a variable)
    /// </summary>
    private static void Identifier(string source)
    {
        while (ScanUtils.IsAlphaNumeric(Peek(source)))
        {
            Advance(source);
        }

        string text = source[start..current];
        if (!LoxKeywords.GetTokenType(text, out TokenType type))
        {
            type = TokenType.IDENTIFIER;
        }
        AddToken(source, type);
    }

    /// <summary>
    /// Used to create a Token with the Tokentype String (a string literal)
    /// </summary>
    private static void AString(string source)
    {
        while (Peek(source) is not '\"' && !IsAtEnd(source))
        {
            if (Peek(source) is '\n')
            {
                line++;
            }
            Advance(source);
        }

        if (IsAtEnd(source))
        {
            LoxErrorLogger.Error(line, "Unterminated string.");
            return;
        }
        Advance(source);

        // Trims the surrounding quotes.
        string value = source.Substring(start + 1, current - start - 2);
        AddToken(source, TokenType.STRING, value);
    }

    /// <summary>
    /// Used to create a Token with the Tokentype Number (a number literal)
    /// </summary>
    private static void Number(string source)
    {
        int digitsAfterPoint = 0;

        while (ScanUtils.IsDigit(Peek(source)))
        {
            Advance(source);
        }

        // Look for a fractional part.
        if (Peek(source) is '.' && ScanUtils.IsDigit(PeekNext(source)))
        {
            // Consumes the "."
            Advance(source);
            while (ScanUtils.IsDigit(Peek(source)))
            {
                Advance(source);
                digitsAfterPoint++;
            }
        }
        double number = Convert.ToDouble(source[start..current]) / Math.Pow(10, digitsAfterPoint);
        AddToken(source, TokenType.NUMBER, number);
    }

    /// <summary>
    /// ´Returns the char one position ahead of the current Position
    /// </summary>
    private static char PeekNext(string source)
    {
        if (current + 1 >= source.Length)
        {
            return '\0';
        }
        return source[current + 1];
    }

    /// <summary>
    /// Returns the char at the current position
    /// </summary>
    private static char Peek(string source)
    {
        if (IsAtEnd(source))
        {
            return '\0';
        }
        return source[current];
    }

    /// <summary>
    /// Matches the char of the current position with the passed char
    /// </summary>
    private static bool Match(string source, char expected)
    {
        if (IsAtEnd(source))
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

    /// <summary>
    /// Advances a postion further in the sourceCode (one character) and returns the character at the previous position
    /// </summary>
    private static char Advance(string source) => source[current++];
}
