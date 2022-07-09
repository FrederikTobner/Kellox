using System;
using System.IO;
using Xunit;

namespace KelloxTests;

public class LiteralTest : KelloxTest
{
    //Folder of the test programs
    protected override string ProjectPath { get; init; } = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}{Path.DirectorySeparatorChar}Literals";

    protected override string FolderName { get; init; } = "TestPrograms";

    /// <summary>
    /// Output of the numbers literal test program
    /// </summary>
    private static readonly string expectedNumberLiteralsOutput = $"123{Environment.NewLine}987654{Environment.NewLine}0{Environment.NewLine}-0{Environment.NewLine}{123.456}{Environment.NewLine}{-0.001}{Environment.NewLine}";

    /// <summary>
    /// Output of the boolean literal test program
    /// </summary>
    private static readonly string expectedBooleanLiteralsOutput = $"true{Environment.NewLine}false{Environment.NewLine}";


    /// <summary>
    /// Output of the string literal test program
    /// </summary>
    private static readonly string expectedStringLiteralsOutput = $"{Environment.NewLine}a{Environment.NewLine}A{Environment.NewLine}123{Environment.NewLine}!\"$%&/()=?{Environment.NewLine}";

    [Fact]
    public void TestNumberLiterals()
    {
        RunTestFile(CreateKelloxTestFilePath("NumberLiterals.klx"));
        Assert.Equal(expectedNumberLiteralsOutput, output.ToString());
    }

    [Fact]
    public void TestBooleanLiterals()
    {
        RunTestFile(CreateKelloxTestFilePath("BooleanLiterals.klx"));
        Assert.Equal(expectedBooleanLiteralsOutput, output.ToString());
    }

    [Fact]
    public void TestStringLiterals()
    {
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
