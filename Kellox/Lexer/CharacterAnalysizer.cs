﻿namespace Kellox.Lexer;

/// <summary>
/// Anaylizes a single character
/// </summary>
internal static class CharacterAnalysizer
{
    /// <summary>
    /// Determines whether a char is from the Alphabet or a underscore
    /// </summary>
    /// <param name="c">The char that is evaluated</param>
    public static bool IsAlpha(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';

    /// <summary>
    /// Determines whether a char is a number (0-9)
    /// </summary>
    /// <param name="c">The char that is evaluated</param>
    public static bool IsDigit(char c) => c is >= '0' and <= '9';

    /// <summary>
    /// Determines whether a char is from the Alphabet, a number or an underscore
    /// </summary>
    /// <param name="c">The char that is evaluated</param>
    public static bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c) || c is '_';
}
