namespace Kellox.Lexer;

/// <summary>
/// Anaylizes a single character
/// </summary>
internal static class CharacterAnalysizer
{
    /// <summary>
    /// Determines weather a char is from the Alphabet or a underscore
    /// </summary>
    /// <param name="c">The char that is evaluated</param>
    public static bool IsAlpha(char c) => c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';

    /// <summary>
    /// Determines weather a char is a number (0-9)
    /// </summary>
    /// <param name="c">The char that is evaluated</param>
    public static bool IsDigit(char c) => c >= '0' && c <= '9';

    /// <summary>
    /// Determines weather a char is from the Alphabet, a number or an underscore
    /// </summary>
    /// <param name="c">The char that is evaluated</param>
    public static bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c) || c is '_';
}
