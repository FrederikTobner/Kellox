using Kellox.Interpreter;
using Kellox.Lexer;
using Kellox.Parser;
using Kellox.Resolver;
using Kellox.Statements;
using Kellox.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace KelloxTests;
/// <summary>
/// KelloxTests affect each other so we have to use a collection to prevent the tests from running in parallel
/// </summary>
[KelloxTestAttribute, Collection("KelloxTest")]
public abstract class KelloxTest
{
    //Folder of the test programs
    protected abstract string ProjectPath { get; init; }

    //Used to redirect the console output
    protected readonly StringWriter output;

    //Initial setup shared among all kellox tests
    public KelloxTest()
    {
        // We create a new StringWriter for each test
        output = new();
        //and redirect the Console output to the StringWriter
        Console.SetOut(output);
    }

    protected string CreateKelloxTestFilePath(string filename)
    {
        string s = "";
        try
        {
            s = Path.Combine(ProjectPath, "TestPrograms", filename);
        }
        catch (Exception)
        {

        }
        return s;
    }

    protected static void RunTestFile(string path)
    {
        byte[]? file;
        //Try catch block for reading a file
        try
        {
            file = File.ReadAllBytes(path);
        }
        //Catches Argument and ArgumentNullException
        catch (Exception)
        {
            Console.WriteLine("File not found");
            return;
        }
        string sourceCode = Encoding.UTF8.GetString(file);
        RunTest(sourceCode);
    }

    protected static void RunTest(string sourceCode)
    {
        IReadOnlyList<Token> tokens = KelloxLexer.ScanTokens(sourceCode);
        // Error during the lexical analysis
        if (KelloxInterpreter.ErrorOccurred)
        {
            return;
        }
        KelloxProgram program = KelloxParser.Parse(tokens);
        // Error during the Parsing process
        if (!program.Runnable)
        {
            return;
        }
        KelloxResolver.Resolve(program);
        // Error during the resolution
        if (!program.Runnable)
        {
            return;
        }
        RunTestProgram(program);
    }

    //runtime errors are not catched for tests
    private static void RunTestProgram(KelloxProgram program)
    {
        foreach (IStatement statement in program)
        {
            statement.Execute();
        }
    }
}
