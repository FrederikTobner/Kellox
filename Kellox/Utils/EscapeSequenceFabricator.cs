using Kellox.Exceptions;
using Kellox.Tokens;
using System.Text;
using System.Text.RegularExpressions;

namespace Kellox.Utils;

/// <summary>
/// Provides methods for handling escape sequences in strings
/// </summary>
internal static class EscapeSequenceFabricator
{
    /// <summary>
    /// Regex for matching an unknown escape seuqence
    /// </summary>
    private const string UnknownEscapeSequencePattern = @"\\[^abfnrtv\""\'\\]";

    /// <summary>
    /// Enriches a string that contains escape sequences
    /// </summary>
    /// <param name="input">The string that shall be enriched</param>
    /// <param name="printToken">The Token of the print statement - used for error reporting</param>
    /// <returns>An enriched version of the Input</returns>
    internal static string EnrichString(string input, Token printToken)
    {
        //Does contain an unknown escape sequence
        if (Regex.IsMatch(input, UnknownEscapeSequencePattern))
        {
            ReportUnknownEscapeSequence(input, printToken);
        }
        StringBuilder stringBuilder = new(input);
        //Alarm bell 🔔
        stringBuilder.Replace("\\a", "\a");
        // Backspace
        stringBuilder.Replace("\\b", "\b");
        //Form Feed
        stringBuilder.Replace("\\f", "\f");
        //Line feed
        stringBuilder.Replace("\\n", "\n");
        //Carrage return
        stringBuilder.Replace("\\r", "\r");
        //Horizontal tab
        stringBuilder.Replace("\\t", "\t");
        //Vertical Tab
        stringBuilder.Replace("\\v", "\v");
        //Single quote
        stringBuilder.Replace("\\\'", "\'");
        //Double quote
        stringBuilder.Replace("\\\"", "\"");
        //Backslash itself
        stringBuilder.Replace("\\\\", "\\");
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Reports an unknown escape sequence in a string
    /// </summary>
    /// <param name="input">The string that contains the unknown escape sequence</param>
    /// <param name="printToken">The Token of the print statement - used for error reporting</param>
    /// <exception cref="RunTimeError"></exception>
    private static void ReportUnknownEscapeSequence(string input, Token printToken)
    {
        StringBuilder errorMessageBuilder = new("unknown escape sequence");
        MatchCollection list = Regex.Matches(input, UnknownEscapeSequencePattern, RegexOptions.Multiline);
        //Multiple unknown escape sequences in the string 😧
        if (list.Count > 1)
        {
            errorMessageBuilder.Append('s');
        }
        for (int i = 0; i < list.Count; i++)
        {
            Match patternMatch = list[i];
            //FirstMatch, MiddleMatch or Last Match?
            errorMessageBuilder.Append(i == 0 ? ' ' : i != list.Count - 1 ? ", " : " and ");
            errorMessageBuilder.Append(patternMatch.ToString());
        }
        throw new RunTimeError(printToken, errorMessageBuilder.ToString());
    }
}
