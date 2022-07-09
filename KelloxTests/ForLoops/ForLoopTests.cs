using System;
using System.IO;
using Xunit;

namespace KelloxTests;

public class ForLoopTests : KelloxTest
{
    //Folder of the test programs
    protected override string ProjectPath { get; init; } = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}{Path.DirectorySeparatorChar}ForLoops";

    protected override string FolderName { get; init; } = "TestPrograms";

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
        RunTestFile(CreateKelloxTestFilePath("BreakTest.klx"));
        Assert.Equal(expectedBreakOutput, output.ToString());
    }

    [Fact]
    public void TestContinueProgram()
    {
        RunTestFile(CreateKelloxTestFilePath("ContinueTest.klx"));
        Assert.Equal(expectedContinueOutput, output.ToString());
    }

    [Fact]
    public void TestSingleExpressionForProgram()
    {
        RunTestFile(CreateKelloxTestFilePath("SingleExpressionForTest.klx"));
        Assert.Equal(expectedSingleExpressionForOutput, output.ToString());
    }

    [Fact]
    public void TestNormalForProgram()
    {
        RunTestFile(CreateKelloxTestFilePath("NormalForTest.klx"));
        Assert.Equal(expectedNormalForOutput, output.ToString());
    }
}
