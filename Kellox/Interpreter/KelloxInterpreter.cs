using Kellox.Exceptions;
using Kellox.Expressions;
using Kellox.Interpreter.Arguments;
using Kellox.Lexer;
using Kellox.Parser;
using Kellox.Resolver;
using Kellox.Statements;
using Kellox.Tokens;
using Kellox.Utils;
using System.Text;

namespace Kellox.Interpreter;

/// <summary>
/// Contains method to interpret a KelloxProgram that was produced by the 
/// </summary>
internal static class KelloxInterpreter
{
    /// <summary>
    /// Boolean value indicating wheather an error has ocurred during the interpretation of the program
    /// </summary>
    public static bool ErrorOccurred { get; internal set; }

    /// <summary>
    /// Boolean value indicating wheather an error has ocurred while running the program
    /// </summary>
    public static bool RunTimeErrorOccurred { get; internal set; }

    /// <summary>
    /// The current Environment where the program is getting executed
    /// </summary>
    internal readonly static KelloxEnvironment globalEnvironment = KelloxEnvironmentInitializer.InitializeGlobal();


    /// <summary>
    /// Contains the local (not global) expressions, that reference a variabale from an outer scope
    /// </summary>
    private static readonly Dictionary<IExpression, int> locals = new();


    /// <summary>
    /// The current Environment, where the program is getting executed
    /// </summary>
    internal static KelloxEnvironment currentEnvironment = globalEnvironment;

    /// <summary>
    /// Boolean value indicating weather the file shall only be analyzed
    /// </summary>
    private static bool onlyAnalyze = false;

    /// <summary>
    /// Starts the interpreter from file or as command prompt, if no file is specified
    /// </summary>
    /// <param name="args">The arguments provided by the user when the interpreter was started</param>
    internal static void RunKelloxInterpreter(params string[] args)
    {
        List<string> options = new();
        string? specifiedFile = null;
        List<string> kelloxArgs = new();
        for (int i = 0; i < args.Length; i++)
        {
            try
            {
                ArgumentFabricator.GroupArgs(options, ref specifiedFile, kelloxArgs, args);
            }
            catch (ArgumentError argumentError)
            {
                Console.WriteLine(argumentError.Message);
                Environment.Exit(64);
            }
        }
        if (specifiedFile is null && options.Count is 0)
        {
            PrintKelloxVersion();
            RunPrompt();
        }

        foreach (string? option in options)
        {
            switch (option)
            {
                case "-h" or "--help":
                    PrintKelloxHelp();
                    Environment.Exit(0);
                    break;
                case "-v" or "--version":
                    PrintKelloxVersion();
                    Environment.Exit(0);
                    break;
                case "-a":
                    if (specifiedFile is null)
                    {
                        Console.WriteLine("file not specified, but option \'-a\' used");
                        PrintKelloxHelp();
                        //Exit code 64 -> The command was used incorrectly, unsupported options
                        Environment.Exit(64);
                    }
                    onlyAnalyze = true;
                    break;
                default:
                    Console.WriteLine("unknown option: " + option);
                    PrintKelloxHelp();
                    //Exit code 64 -> The command was used incorrectly, unsupported options
                    Environment.Exit(64);
                    break;
            }
        }

        if (specifiedFile is not null)
        {
            RunFile(specifiedFile);
        }
    }

    private static void PrintKelloxHelp()
    {
        Console.WriteLine("usage: kellox [-h|-v||[-c file| file]]?");
        Console.WriteLine("These are common Kellox Interpreter commands:\nRun from Prompt\nkellox\nRun a Kellox file\nkellox file.klx\nAnaylize file\nkellox -a file.klx");
    }

    private static void PrintKelloxVersion()
    {
        Console.WriteLine("Kellox 0.1");
    }

    private static void PrintPrompt()
    {
        Console.Write(">>> ");
    }

    /// <summary>
    /// Executes a Kellox program from a file
    /// </summary>
    private static void RunFile(string path)
    {
        byte[] file = File.ReadAllBytes(path);
        Run(Encoding.UTF8.GetString(file));
        if (RunTimeErrorOccurred)
        {
            // An internal software error has been detected
            Environment.Exit(70);
        }
        if (ErrorOccurred)
        {
            // The input data/kellox programm was incorrect -> Error during the lexical analysis or the parsing process
            Environment.Exit(65);
        }
    }

    /// <summary>
    /// Executes a Kellox program from the Command prompt
    /// </summary>
    private static void RunPrompt()
    {
        PrintPrompt();
        string? line = Console.ReadLine();
        // If the user hasn't given any input the console will close
        while (line is not "" && line is not null)
        {
            Run(line);
            ErrorOccurred = false;
            // Adds linebreak if the writing position in the console is not at the left border
            if (Console.GetCursorPosition().Left is not 0)
            {
                Console.Write(Environment.NewLine);
            }
            PrintPrompt();
            line = Console.ReadLine();
        }
    }

    /// <summary>
    /// Executes a Kellox program
    /// </summary>
    /// <param name="sourceCode">The sourcecode of the program that shall be executed</param>
    private static void Run(string sourceCode)
    {
        IReadOnlyList<Token> tokens = KelloxLexer.ScanTokens(sourceCode);
        // Error during the lexical analysis
        if (ErrorOccurred)
        {
            return;
        }
        KelloxProgram program = KelloxParser.Parse(tokens);
        // Error during the Parsing process
        if (!program.Runnable || onlyAnalyze)
        {
            if (onlyAnalyze && program.Runnable)
            {
                Console.WriteLine("Syntax OK");
            }
            return;
        }
        KelloxResolver.Resolve(program);
        // Error during the resolution
        if (!program.Runnable)
        {
            return;
        }
        RunProgram(program);
    }

    /// <summary>
    /// Executes the Program written in Kellox after the variables in the inner scopes have been resolved
    /// </summary>
    /// <param name="program">The program that is executed</param>
    private static void RunProgram(KelloxProgram program)
    {
        foreach (IStatement statement in program)
        {
            try
            {
                statement.Execute();
            }
            catch (RunTimeError runTimeError)
            {
                ReportRunTimeError(runTimeError);
                //Program execution stops after an error at runtime has occured
                break;
            }
        }
    }

    /// <summary>
    /// Stores the distance beetween the scopes, that local (not global) variables are referenced and declared
    /// </summary>
    /// <param name="expression">The Expression referencing a Variable</param>
    /// <param name="distance">The distance between the scopes</param>
    internal static void Resolve(IExpression expression, int distance)
    {
        locals.Add(expression, distance);
    }

    internal static bool TryGetDepthOfLocal(IExpression expression, out int distance) => locals.TryGetValue(expression, out distance);


    /// <summary>
    /// Reports a error that has orrured during runtime
    /// </summary>
    /// <param name="runTimeError">The Error that has occured during runtime</param>
    private static void ReportRunTimeError(RunTimeError runTimeError)
    {
        RunTimeErrorOccurred = true;
        ErrorLogger.Error(runTimeError.Token.Line, runTimeError.Message);
    }
}
