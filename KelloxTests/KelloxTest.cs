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
    private static readonly string projectPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}";

    private static readonly string FolderName = "TestPrograms";

    //Used to redirect the console output
    protected readonly StringWriter output;

    //Initial setup shared among all kellox tests
    public KelloxTest()
    {
        // We create a new StringWriter for each test
        output = new();
        //We redirect the Console output to the StringWriter
        Console.SetOut(output);
    }

    protected static string CreateKelloxTestFilePath(string filename)
    {
        string s = "";
        try
        {
            s = Path.Combine(projectPath, FolderName, filename);
        }
        catch (Exception)
        {

        }
        return s;
    }
}
