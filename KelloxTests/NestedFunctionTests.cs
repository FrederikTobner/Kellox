using Kellox.Expressions;
using Kellox.Interpreter;
using Kellox.Tokens;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace KelloxTests;

public class NestedFunctionTests
{
    /// <summary>
    /// Resets Environment after each test execution
    /// </summary>
    public NestedFunctionTests()
    {
        KelloxInterpreter.ResetEnvironment();
    }

    /// <summary>
    /// Path of the Fibonacci Test Program
    /// </summary>
    private static readonly string fibonacciProgramPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}\\TestPrograms\\FibonacciProgram.klx";

    /// <summary>
    /// The first 25 fibonacci numbers stored in an array
    /// </summary>
    private static readonly double[] fibonacciNumbers = { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 17711, 28657, 46368 };

    [Fact]
    public void TestFibonacciProgram()
    {
        CallExpression fibonacciFunction = new(
        new VariableExpression(
            new Token(TokenType.VAR, "fibo", null, 0)),
            new Token(TokenType.RIGHT_PARENTHESIS, ")", null, 0),
            new());
        string program = Encoding.UTF8.GetString(File.ReadAllBytes(fibonacciProgramPath));
        KelloxInterpreter.Run(program, false);
        for (int i = 0; i < 25; i++)
        {
            Assert.Equal(fibonacciNumbers[i], fibonacciFunction.Evaluate());
        }

    }
}
