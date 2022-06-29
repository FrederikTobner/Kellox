using Kellox.Interpreter;
using Kellox.Tokens;

namespace Kellox.Utils;

/// <summary>
/// Class contains methods to report an error during the parsing process, during the lexical analysis, during the resolution of the abstract syntax tree or at runtime 😨
/// </summary>
internal static class ErrorLogger
{
    /// <summary>
    /// Reports an error in specific line
    /// </summary>
    internal static void Error(int line, string message) => Report(line, "", message);

    /// <summary>
    /// Reports an error that was caused by a specific token
    /// </summary>
    internal static void Error(Token token, string message)
    {
        switch (token.TokenType)
        {
            case TokenType.EOF:
                Report(token.Line, " at end of file", message);
                break;
            default:
                Report(token.Line, $" at '{token.Lexeme}'", message);
                break;
        }
    }

    /// <summary>
    /// Reports an error that occured during the lexical analysis
    /// </summary>
    /// <param name="line">The line where the error has occured</param>
    /// <param name="where">A description where the Error occured</param>
    /// <param name="message">The Error Message that is displayed</param>
    private static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error{where} : {message}");
        KelloxInterpreter.ErrorOccurred = true;
    }
}
