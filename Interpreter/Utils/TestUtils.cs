using Interpreter.Expressions;

namespace Interpreter.Utils
{
    internal static class TestUtils
    {
        /// <summary>
        /// Path of the SampleProgram
        /// </summary>
        private static readonly string sampleProgramPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}\\SampleProgram.txt";

        /// <summary>
        /// Tests the interrpreter by running the sample Program
        /// </summary>
        internal static void TestSampleProgram() => RunnerUtils.RunFile(sampleProgramPath);

        /// <summary>
        /// Tests printing of a nested Expression
        /// </summary>
        internal static void TestExpression()
        {
            IExpression expression = new BinaryExpression(
            new UnaryExpression(
                new Token(TokenType.MINUS, "-", null, 1),
                new LiteralExpression(123)),
            new Token(TokenType.STAR, "*", null, 1),
            new GroupingExpression(
                new LiteralExpression(45.67)));
            Console.WriteLine(expression.ToString());
        }
    }
}
