using Lox.Expressions;
using Lox.Tokens;

namespace Lox.Utils
{
    /// <summary>
    /// Contains methods used for testing
    /// </summary>
    internal static class LoxTests
    {
        /// <summary>
        /// Path of the SampleProgram
        /// </summary>
        private static readonly string sampleProgramPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}\\SamplePrograms\\SampleProgram.txt";

        /// <summary>
        /// Tests the interrpreter by running the sample Program
        /// </summary>
        internal static void RunSampleProgram() => LoxRunner.RunFile(sampleProgramPath);

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
