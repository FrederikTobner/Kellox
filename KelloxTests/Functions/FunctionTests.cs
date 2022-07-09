using System;
using System.IO;
using Xunit;

namespace KelloxTests.Functions;

public class FunctionTests : KelloxTest
{
    //Folder of the test programs
    protected override string ProjectPath { get; init; } = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}{Path.DirectorySeparatorChar}Functions";

    [Fact]
    public void TestNestedFunction()
    {
        string expectedNestedFunctionOutput = $"0{Environment.NewLine}1{Environment.NewLine}1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}5{Environment.NewLine}8{Environment.NewLine}13{Environment.NewLine}21{Environment.NewLine}34{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("NestedFunction.klx"));
        Assert.Equal(expectedNestedFunctionOutput, output.ToString());
    }

    [Fact]
    public void TestEmptyBody()
    {
        RunTestFile(CreateKelloxTestFilePath("EmptyBody.klx"));
        Assert.Equal("nil", output.ToString());
    }

    [Fact]
    public void TestParameters()
    {
        string expectedParametersOutput = $"0{Environment.NewLine}1{Environment.NewLine}3{Environment.NewLine}6{Environment.NewLine}10{Environment.NewLine}15{Environment.NewLine}21{Environment.NewLine}28{Environment.NewLine}36{Environment.NewLine}";
        RunTestFile(CreateKelloxTestFilePath("Parameters.klx"));
        Assert.Equal(expectedParametersOutput, output.ToString());
    }

    [Fact]
    public void TestRecursion()
    {
        RunTestFile(CreateKelloxTestFilePath("Recursion.klx"));
        Assert.Equal("21", output.ToString());
    }
}
