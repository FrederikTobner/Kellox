using Kellox.Interpreter;
using System;
using System.IO;
using Xunit;

namespace KelloxTests;

public class FunctionTests : KelloxTest
{
    /// <summary>
    /// Path of the Fibonacci Test Program
    /// </summary>
    private static readonly string fibonacciProgramPath = CreateKelloxTestFilePath("FibonacciProgram.klx");

    /// <summary>
    /// Path of the wrong argument count Test Program
    /// </summary>
    private static readonly string emptyBodyProgramPath = CreateKelloxTestFilePath("EmptyBody.klx");

    /// <summary>
    /// Path of the parameters Test Program
    /// </summary>
    private static readonly string parametersProgramPath = CreateKelloxTestFilePath("Parameters.klx");

    /// <summary>
    /// Path of the parameters Test Program
    /// </summary>
    private static readonly string recursionProgramPath = CreateKelloxTestFilePath("Recursion.klx");

    /// <summary>
    /// Output of the fibonacci program
    /// </summary>
    private static readonly string expectedFibonacciOutput = $"0{Environment.NewLine}1{Environment.NewLine}1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}5{Environment.NewLine}8{Environment.NewLine}13{Environment.NewLine}21{Environment.NewLine}34{Environment.NewLine}";

    /// <summary>
    /// Output of the parameters program
    /// </summary>
    private static readonly string expectedParametersOutput = $"0{Environment.NewLine}1{Environment.NewLine}3{Environment.NewLine}6{Environment.NewLine}10{Environment.NewLine}15{Environment.NewLine}21{Environment.NewLine}28{Environment.NewLine}36{Environment.NewLine}";

    [Fact]
    public void TestNestedFunction()
    {
        KelloxInterpreter.RunFile(fibonacciProgramPath);
        Assert.Equal(expectedFibonacciOutput, output.ToString());
    }

    [Fact]
    public void TestEmptyBody()
    {
        KelloxInterpreter.RunFile(emptyBodyProgramPath);
        Assert.Equal("nil", output.ToString());
    }

    [Fact]
    public void TestParameters()
    {
        KelloxInterpreter.RunFile(parametersProgramPath);
        Assert.Equal(expectedParametersOutput, output.ToString());
    }

    [Fact]
    public void TestRecursion()
    {
        KelloxInterpreter.RunFile(recursionProgramPath);
        Assert.Equal("21", output.ToString());
    }
}
