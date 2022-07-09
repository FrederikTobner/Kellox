using System;
using System.IO;
using Xunit;

namespace KelloxTests.Literals;

public class LiteralTest : KelloxTest
{
    //Folder of the test programs
    protected override string ProjectPath { get; init; } = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}{Path.DirectorySeparatorChar}Literals";

    [Fact]
    public void TestNumberLiterals()
    {
        string expectedNumberLiteralsOutput = $"123{Environment.NewLine}987654{Environment.NewLine}0{Environment.NewLine}-0{Environment.NewLine}{123.456}{Environment.NewLine}{-0.001}{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("NumberLiterals.klx"));
        Assert.Equal(expectedNumberLiteralsOutput, output.ToString());
    }

    [Fact]
    public void TestBooleanLiterals()
    {
        string expectedBooleanLiteralsOutput = $"true{Environment.NewLine}false{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("BooleanLiterals.klx"));
        Assert.Equal(expectedBooleanLiteralsOutput, output.ToString());
    }

    [Fact]
    public void TestStringLiterals()
    {
        string expectedStringLiteralsOutput = $"{Environment.NewLine}a{Environment.NewLine}A{Environment.NewLine}123{Environment.NewLine}!\"$%&/()=?{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("StringLiterals.klx"));
        Assert.Equal(expectedStringLiteralsOutput, output.ToString());
    }

    [Fact]
    public void TestNilLiteral()
    {
        RunTest("print nil;");
        Assert.Equal("nil", output.ToString());
    }
}
