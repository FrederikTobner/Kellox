using Kellox.Exceptions;
using Kellox.Expressions;
using Kellox.Tokens;
using System.Text;
using System.Text.RegularExpressions;

namespace Kellox.Statements;

/// <summary>
/// Models a print statement
/// </summary>
internal class PrintStatement : IStatement
{
    /// <summary>
    /// The Expression that shall be printed e.g "Hallo" / 6.3
    /// </summary>
    public IExpression Expression { get; init; }

    /// <summary>
    /// Regex for matching an unknown escape seuqence
    /// </summary>
    private const string UnknownEscapeSequencePattern = @"\\[^abfnrtv\""\'\\]";

    /// <summary>
    /// Boolean value that determines weather a line break will be added after the printing the Expression
    /// </summary>
    private readonly bool addLineBreak;

    /// <summary>
    /// Token of the print statement used for error logging
    /// </summary>
    private readonly Token printToken;

    public PrintStatement(IExpression expression, Token printToken, bool addLineBreak = false)
    {
        this.Expression = expression;
        this.printToken = printToken;
        this.addLineBreak = addLineBreak;

    }

    public void Execute()
    {
        object? obj = Expression.Evaluate();
        if (obj is null)
        {
            Console.Write(addLineBreak ? "nil" + Environment.NewLine : "nil");
            return;
        }
        if (obj is not string)
        {
            Console.Write(addLineBreak ? obj + Environment.NewLine : obj);
            return;
        }
        string text = (string)obj;

        //Contains escape sequences
        if (text.Contains('\\'))
        {
            //Does contain an unknown escape sequence
            if (Regex.IsMatch(text, UnknownEscapeSequencePattern))
            {
                StringBuilder errorMessageBuilder = new("unknown escape sequence");
                MatchCollection list = Regex.Matches(text, UnknownEscapeSequencePattern, RegexOptions.Multiline);
                //Multiple unknown escape sequences in the string
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
            StringBuilder stringBuilder = new(text);
            //Alarm bell
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
            Console.Write(addLineBreak ? stringBuilder.ToString() + Environment.NewLine : stringBuilder.ToString());
            return;
        }
        Console.Write(addLineBreak ? text + Environment.NewLine : text);
        return;
    }
}
