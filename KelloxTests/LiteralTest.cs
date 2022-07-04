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

    [Fact]
    public void TestNumberLiterals()
    {
        KelloxInterpreter.RunFile(numberLiteralsProgramPath);
        Assert.Equal(expectedNumberLiteralsOutput, output.ToString());
    }
}
