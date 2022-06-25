using Kellox.Exceptions;
using Kellox.Expressions;
using Kellox.Lexer;
using Kellox.Parser;
using Kellox.Resolver;
using Kellox.Statements;
using Kellox.Tokens;
using Kellox.Utils;
using System.Text;
using System.Text.RegularExpressions;

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

    private const string KelloxFilePattern = @".klx+$";

    /// <summary>
    /// Starts the interpreter from file or as command prompt, if no file is specified
    /// </summary>
    /// <param name="args">The arguments provided by the user when the interpreter was started</param>
    internal static void RunKelloxInterpreter(params string[] args)
    {
        if (args.Length is 0)
        {
            RunPrompt();
        }
        if (args.Length > 2)
        {
            Console.WriteLine("To much arguments");
            //Exit code 64 -> The command was used incorrectly, wrong number of anguments
            Environment.Exit(64);
        }
        if (args.Length is 2)
        {
            if (args[1][0] is '-')
            {
                if (args[1] is "-c")
                {
                    CheckSyntaxOfFile(args[1]);
                }
            }
        }
        if (args.Length is 1)
        {
            if (args[0][0] is '-')
            {
                RunWithGenericOption(args[0]);
                return;
            }
            RunFile(args[0]);
        }
    }


    private static void RunWithGenericOption(string option)
    {
        switch (option)
        {
            case "-h" or "--help":
                Console.WriteLine("Kellox Help");
                break;
            case "-v" or "--version":
                Console.WriteLine("Kellox 1.0");
                break;
            default:
                Console.WriteLine("Unsupported Option");
                Environment.Exit(65);
                break;
        }
    }


    private static void CheckSyntaxOfFile(string path)
    {
        if (!Regex.IsMatch(path, KelloxFilePattern))
        {
            Console.WriteLine("File at " + path + " is not a kellox file");
            Environment.Exit(64);
        }
        byte[] file = File.ReadAllBytes(path);
        string sourceCode = Encoding.UTF8.GetString(file);
        IReadOnlyList<Token> tokens = KelloxLexer.ScanTokens(sourceCode);
        // Error during the lexical analysis
        if (ErrorOccurred)
        {
            return;
        }
        KelloxProgram program = KelloxParser.Parse(tokens);
        // Error during the Parsing process
        if (program.Runnable)
        {
            Console.WriteLine("Syntax OK");
            return;
        }
    }

    /// <summary>
    /// Executes a Kellox program from a file
    /// </summary>
    private static void RunFile(string path)
    {
        if (!Regex.IsMatch(path, KelloxFilePattern))
        {
            Console.WriteLine("File at " + path + " is not a kellox file");
            Environment.Exit(64);
        }
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
        Console.WriteLine("> ");
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
            Console.WriteLine("> ");
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
