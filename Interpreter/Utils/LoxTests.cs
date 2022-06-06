using Interpreter.Expressions;

namespace Interpreter.Utils
{
    /// <summary>
    /// Contains methods used for testing
    /// </summary>
    internal static class LoxTests
    {
        /// <summary>
        /// Path of the SampleProgram
        /// </summary>
        private static readonly string sampleProgramPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}\\SampleProgram.txt";

        /// <summary>
        /// Tests the interrpreter by running the sample Program
        /// </summary>
        internal static void SampleProgram() => LoxRunner.RunFile(sampleProgramPath);

        /// <summary>
        /// Tests printing of a nested Expression
        /// </summary>
        internal static void Expression()
        {
            IExpression expression = new BinaryExpression(
            new UnaryExpression(
                new Token(TOKENTYPE.MINUS, "-", null, 1),
                new LiteralExpression(123)),
            new Token(TOKENTYPE.STAR, "*", null, 1),
            new GroupingExpression(
                new LiteralExpression(45.67)));
            Console.WriteLine(expression.ToString());
        }
    }
}
