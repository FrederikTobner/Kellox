using Kellox.Interpreter;
using System;
using Xunit;

namespace KelloxTests;

public class FunctionTests : KelloxTest
{
    /// <summary>
    /// Path of the Fibonacci Test Program
    /// </summary>
    private static readonly string fibonacciProgramPath = TestProgramFolderPath + "FibonacciProgram.klx";

    /// <summary>
    /// Output of the fibonacci program
    /// </summary>
    private static readonly string expectedFibonacciOutput = $"0{Environment.NewLine}1{Environment.NewLine}1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}5{Environment.NewLine}8{Environment.NewLine}13{Environment.NewLine}21{Environment.NewLine}34{Environment.NewLine}";

    [Fact]
    public void TestFibonacciProgram()
    {
        KelloxInterpreter.RunFile(fibonacciProgramPath);
        Assert.Equal(expectedFibonacciOutput, output.ToString());
    }
}
