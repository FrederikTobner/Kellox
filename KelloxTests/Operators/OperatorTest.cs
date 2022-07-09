using System;
using System.IO;
using Xunit;

namespace KelloxTests.Operators;

public class OperatorTest : KelloxTest
{
    protected override string ProjectPath { get; init; } = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}{Path.DirectorySeparatorChar}Operators";

    [Fact]
    public void TestPlusOperator()
    {
        string expectedPlusOperatorOutput = $"579{Environment.NewLine}{123.45}{Environment.NewLine}hello{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("PlusOperator.klx"));
        Assert.Equal(expectedPlusOperatorOutput, output.ToString());
    }

    [Fact]
    public void TestDivideOperator()
    {
        string expectedDivideOperatorOutput = $"4{Environment.NewLine}1{Environment.NewLine}{5.5}{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("DivideOperator.klx"));
        Assert.Equal(expectedDivideOperatorOutput, output.ToString());
    }

    [Fact]
    public void TestMultiplyOperator()
    {
        string expectedMultiplyOperatorOutput = $"15{Environment.NewLine}{3.702}{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("MultiplyOperator.klx"));
        Assert.Equal(expectedMultiplyOperatorOutput, output.ToString());
    }

    [Fact]
    public void TestSubtractOperator()
    {
        string expectedSubtractOperatorOutput = $"1{Environment.NewLine}0{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("SubtractOperator.klx"));
        Assert.Equal(expectedSubtractOperatorOutput, output.ToString());
    }
}
