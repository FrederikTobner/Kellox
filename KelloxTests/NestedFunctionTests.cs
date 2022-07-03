using Kellox.Interpreter;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace KelloxTests;

public class NestedFunctionTests
{
    //Used to redirect the console output to the stringWriter
    private readonly StringWriter output = new();

    /// <summary>
    /// Resets Environment after each test execution
    /// </summary>
    public NestedFunctionTests()
    {
        Console.SetOut(output);
    }

    /// <summary>
    /// Path of the Fibonacci Test Program
    /// </summary>
    private static readonly string fibonacciProgramPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}\\TestPrograms\\FibonacciProgram.klx";

    /// <summary>
    /// Path of the Fibonacci Test Program
    /// </summary>
    private static readonly string breakProgramPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}\\TestPrograms\\BreakTest.klx";

    /// <summary>
    /// Path of the Fibonacci Test Program
    /// </summary>
    private static readonly string continueProgramPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}\\TestPrograms\\ContinueTest.klx";

    /// <summary>
    /// Output of the fibonacci program
    /// </summary>
    private static readonly string expectedFibonacciOutput = $"0{Environment.NewLine}1{Environment.NewLine}1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}5{Environment.NewLine}8{Environment.NewLine}13{Environment.NewLine}21{Environment.NewLine}34{Environment.NewLine}";

    /// <summary>
    /// Output of the break program
    /// </summary>
    private static readonly string expectedBreakOutput = $"1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}4{Environment.NewLine}5{Environment.NewLine}6{Environment.NewLine}";

    /// <summary>
    /// Output of the break program
    /// </summary>
    private static readonly string expectedContinueOutput = $"1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}4{Environment.NewLine}5{Environment.NewLine}6{Environment.NewLine}8{Environment.NewLine}9{Environment.NewLine}";

    [Fact, ResetEnvironmentAfterTest]
    public void TestFibonacciProgram()
    {
        string program = Encoding.UTF8.GetString(File.ReadAllBytes(fibonacciProgramPath));
        KelloxInterpreter.Run(program, false);
        Assert.Equal(expectedFibonacciOutput, output.ToString());
    }

    [Fact, ResetEnvironmentAfterTest]
    public void TestBreakProgram()
    {
        string program = Encoding.UTF8.GetString(File.ReadAllBytes(breakProgramPath));
        KelloxInterpreter.Run(program, false);
        Assert.Equal(expectedBreakOutput, output.ToString());
    }

    [Fact, ResetEnvironmentAfterTest]
    public void TestContinueProgram()
    {
        string program = Encoding.UTF8.GetString(File.ReadAllBytes(continueProgramPath));
        KelloxInterpreter.Run(program, false);
        Assert.Equal(expectedContinueOutput, output.ToString());
    }
}
