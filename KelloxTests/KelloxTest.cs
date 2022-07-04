using System;
using System.IO;
using Xunit;

namespace KelloxTests;
/// <summary>
/// KelloxTests affect each other so we have to use a collection to prevent the tests from running in parallel
/// </summary>
[KelloxTestAttribute, Collection("KelloxTest")]
public abstract class KelloxTest
{
    //Folder of the test programs
    protected static readonly string TestProgramFolderPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}\\TestPrograms\\";

    //Used to redirect the console output
    protected readonly StringWriter output;

    //Initial setup shared among all kellox tests
    public KelloxTest()
    {
        output = new();
        Console.SetOut(output);
    }
}
