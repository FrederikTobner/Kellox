using System;
using System.IO;
using Xunit;

namespace KelloxTests.ForLoops;

public class ForLoopTests : KelloxTest
{
    //Folder of the test programs
    protected override string ProjectPath { get; init; } = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}{Path.DirectorySeparatorChar}ForLoops";

    [Fact]
    public void TestBreakProgram()
    {
        string expectedBreakOutput = $"1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}4{Environment.NewLine}5{Environment.NewLine}6{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("Break.klx"));
        Assert.Equal(expectedBreakOutput, output.ToString());
    }

    [Fact]
    public void TestContinueProgram()
    {
        string expectedContinueOutput = $"1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}4{Environment.NewLine}5{Environment.NewLine}6{Environment.NewLine}8{Environment.NewLine}9{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("Continue.klx"));
        Assert.Equal(expectedContinueOutput, output.ToString());
    }

    [Fact]
    public void TestSingleExpressionForProgram()
    {
        string expectedSingleExpressionForOutput = $"1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("SingleExpressionFor.klx"));
        Assert.Equal(expectedSingleExpressionForOutput, output.ToString());
    }

    [Fact]
    public void TestNormalForProgram()
    {
        string expectedNormalForOutput = $"1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}4{Environment.NewLine}5{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("NormalFor.klx"));
        Assert.Equal(expectedNormalForOutput, output.ToString());
    }

    [Fact]
    public void TestNoClausesForProgram()
    {
        string expectedNoClausesOutput = $"1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("NoClauses.klx"));
        Assert.Equal(expectedNoClausesOutput, output.ToString());
    }

    [Fact]
    public void TestNoConditionForProgram()
    {
        string expectedNoConditionOutput = $"0{Environment.NewLine}1{Environment.NewLine}2{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("NoCondition.klx"));
        Assert.Equal(expectedNoConditionOutput, output.ToString());
    }

    [Fact]
    public void TestNoVariableForProgram()
    {
        string expectedNoConditionOutput = $"0{Environment.NewLine}1{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("NoVariable.klx"));
        Assert.Equal(expectedNoConditionOutput, output.ToString());
    }
}
