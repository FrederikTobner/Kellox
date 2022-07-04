using Kellox.Interpreter;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace KelloxTests;

public class FunctionTests : KelloxTests
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
        string program = Encoding.UTF8.GetString(File.ReadAllBytes(fibonacciProgramPath));
        KelloxInterpreter.Run(program, false);
        Assert.Equal(expectedFibonacciOutput, output.ToString());
    }
}
