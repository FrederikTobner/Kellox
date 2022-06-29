namespace Kellox.Tokens;

/// <summary>
/// Models a Token (a word in a programming language)
/// </summary>
public class Token
{
    /// <summary>
    /// The lexeme of the Token, meaning its representation as a string
    /// </summary>
    public string Lexeme { get; init; }

    /// <summary>
    /// The Literal of the Token (null for everything that is not a literal)
    /// </summary>
    public object? Literal { get; init; }

    /// <summary>
    /// The line in the file where the Token occured
    /// </summary>
    public int Line { get; init; }

    /// <summary>
    /// The Type of the Token (e.g. Identifier, +-operator, null/nil-value)
    /// </summary>
    internal TokenType TokenType { get; init; }

    public Token(TokenType tokenType, string lexeme, object? literal, int line)
    {
        TokenType = tokenType;
        Lexeme = lexeme;
        Literal = literal;
        Line = line;
    }

    /// <summary>
    /// Converts the Token to a String
    /// </summary>
    /// <returns></returns>
    public override string ToString() => TokenType.ToString() + " " + Lexeme + " " + Literal;
}
