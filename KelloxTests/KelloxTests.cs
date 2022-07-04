using System;
using System.IO;
using Xunit;

namespace KelloxTests;
/// <summary>
/// KelloxTests affect each other so we have to use a collection to prevent the tests from running in parallel
/// </summary>
[KelloxTestAttribute, Collection("KelloxTest")]
public abstract class KelloxTests
{
    protected static readonly string TestProgramFolderPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}\\TestPrograms\\";

    //Used to redirect the console output to the stringWriter
    protected readonly StringWriter output = new();

    public KelloxTests()
    {
        Console.SetOut(output);
    }
}
