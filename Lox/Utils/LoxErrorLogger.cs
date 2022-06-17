using Lox.Interpreter;
using Lox.Tokens;

namespace Lox.Utils
{
    /// <summary>
    /// Class contains methods to report an error during the ParingProcess/at runtime/durng the semantic analysis
    /// </summary>
    internal static class LoxErrorLogger
    {
        /// <summary>
        /// Reports an error that occured during the lexical analysis
        /// </summary>
        /// <param name="line">The line where the error has occured</param>
        /// <param name="where"></param>
        /// <param name="message"></param>
        private static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error{where}: {message}");
            LoxInterpreter.ErrorOccurred = true;
        }

        /// <summary>
        /// Reports an error that occured during the lexical analysis in specific line
        /// </summary>
        internal static void Error(int line, string message) => Report(line, "", message);

        /// <summary>
        /// Reports an error that occured during the lexical analysis that was caused by a specific token
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
    }
}
