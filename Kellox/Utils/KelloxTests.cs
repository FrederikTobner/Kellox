using Kellox.Expressions;
using Kellox.Interpreter;
using Kellox.Tokens;

namespace Kellox.Utils
{
    /// <summary>
    /// Contains methods used for testing
    /// </summary>
    internal static class KelloxTests
    {
        /// <summary>
        /// Path of the SampleProgram
        /// </summary>
        private static readonly string sampleProgramPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}\\SamplePrograms\\SampleProgram.klx";

        /// <summary>
        /// Tests the interrpreter by running the sample Program
        /// </summary>
        internal static void RunSampleProgram() => KelloxInterpreter.RunLoxInterpreter(sampleProgramPath);

        /// <summary>
        /// Tests printing of a nested Expression
        /// </summary>
        internal static void Expression()
        {
            IExpression expression = new BinaryExpression(
            new UnaryExpression(
                new(TokenType.MINUS, "-", null, 1),
                new LiteralExpression(123)),
            new(TokenType.STAR, "*", null, 1),
            new GroupingExpression(
                new LiteralExpression(45.67)));
            Console.WriteLine(expression.ToString());
        }
    }
}
