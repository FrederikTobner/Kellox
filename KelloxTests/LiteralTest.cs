using Kellox.Interpreter;
using System;
using Xunit;

namespace KelloxTests;

public class LiteralTest : KelloxTest
{
    /// <summary>
    /// Path of the NumberLiterals Test Program
    /// </summary>
    private static readonly string numberLiteralsProgramPath = CreateKelloxTestFilePath("NumberLiterals.klx");

    /// <summary>
    /// Output of the numbers literal test program
    /// </summary>
    private static readonly string expectedNumberLiteralsOutput = $"123{Environment.NewLine}987654{Environment.NewLine}0{Environment.NewLine}-0{Environment.NewLine}{123.456}{Environment.NewLine}{-0.001}{Environment.NewLine}";

    /// <summary>
    /// Path of the BooleanLiterals Test Program
    /// </summary>
    private static readonly string booleanLiteralsProgramPath = CreateKelloxTestFilePath("BooleanLiterals.klx");

    /// <summary>
    /// Output of the boolean literal test program
    /// </summary>
    private static readonly string expectedBooleanLiteralsOutput = $"true{Environment.NewLine}false{Environment.NewLine}";


    /// <summary>
    /// Path of the StringLiterals Test Program
    /// </summary>
    private static readonly string stringLiteralsProgramPath = CreateKelloxTestFilePath("StringLiterals.klx");

    /// <summary>
    /// Output of the string literal test program
    /// </summary>
    private static readonly string expectedStringLiteralsOutput = $"{Environment.NewLine}a{Environment.NewLine}A{Environment.NewLine}123{Environment.NewLine}!\"$%&/()=?{Environment.NewLine}";

    [Fact]
    public void TestNumberLiterals()
    {
        KelloxInterpreter.RunFile(numberLiteralsProgramPath);
        Assert.Equal(expectedNumberLiteralsOutput, output.ToString());
    }

    [Fact]
    public void TestBooleanLiterals()
    {
        KelloxInterpreter.RunFile(booleanLiteralsProgramPath);
        Assert.Equal(expectedBooleanLiteralsOutput, output.ToString());
    }

    [Fact]
    public void TestStringLiterals()
    {
        KelloxInterpreter.RunFile(stringLiteralsProgramPath);
        Assert.Equal(expectedStringLiteralsOutput, output.ToString());
    }

    [Fact]
    public void TestNilLiteral()
    {
        KelloxInterpreter.Run("print nil;");
        Assert.Equal("nil", output.ToString());
    }
}
