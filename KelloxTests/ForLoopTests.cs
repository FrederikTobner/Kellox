using Kellox.Interpreter;
using System;
using System.IO;
using Xunit;

namespace KelloxTests;

public class ForLoopTests : KelloxTest
{
    /// <summary>
    /// Path of the Fibonacci Test Program
    /// </summary>
    private static readonly string breakProgramPath = CreateKelloxTestFilePath("BreakTest.klx");

    /// <summary>
    /// Path of the Fibonacci Test Program
    /// </summary>
    private static readonly string continueProgramPath = CreateKelloxTestFilePath("ContinueTest.klx");

    /// <summary>
    /// Path of the Fibonacci Test Program
    /// </summary>
    private static readonly string singleExpressionForProgramPath = CreateKelloxTestFilePath("SingleExpressionForTest.klx");

    /// <summary>
    /// Path of the Fibonacci Test Program
    /// </summary>
    private static readonly string normalForProgramPath = CreateKelloxTestFilePath("NormalForTest.klx");

    /// <summary>
    /// Output of the break program
    /// </summary>
    private static readonly string expectedBreakOutput = $"1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}4{Environment.NewLine}5{Environment.NewLine}6{Environment.NewLine}";

    /// <summary>
    /// Output of the break program
    /// </summary>
    private static readonly string expectedContinueOutput = $"1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}4{Environment.NewLine}5{Environment.NewLine}6{Environment.NewLine}8{Environment.NewLine}9{Environment.NewLine}";

    /// <summary>
    /// Output of the single expression for loop program
    /// </summary>
    private static readonly string expectedSingleExpressionForOutput = $"1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}";

    /// <summary>
    /// Output of the normal for loop program
    /// </summary>
    private static readonly string expectedNormalForOutput = $"1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}4{Environment.NewLine}5{Environment.NewLine}";

    [Fact]
    public void TestBreakProgram()
    {
        KelloxInterpreter.RunFile(breakProgramPath);
        Assert.Equal(expectedBreakOutput, output.ToString());
    }

    [Fact]
    public void TestContinueProgram()
    {
        KelloxInterpreter.RunFile(continueProgramPath);
        Assert.Equal(expectedContinueOutput, output.ToString());
    }

    [Fact]
    public void TestSingleExpressionForProgram()
    {
        KelloxInterpreter.RunFile(singleExpressionForProgramPath);
        Assert.Equal(expectedSingleExpressionForOutput, output.ToString());
    }

    [Fact]
    public void TestNormalForProgram()
    {
        KelloxInterpreter.RunFile(normalForProgramPath);
        Assert.Equal(expectedNormalForOutput, output.ToString());
    }
}
