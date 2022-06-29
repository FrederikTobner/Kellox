using Kellox.Arguments;
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
public static class KelloxInterpreter
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
    /// Starts the interpreter from file or as command prompt, if no file is specified
    /// </summary>
    /// <param name="args">The arguments provided by the user when the interpreter was started</param>
    internal static void RunKelloxInterpreter(params string[] args)
    {
        List<string> options = new();
        string? specifiedFile = null;
        // TODO Kelloxargs should be available as an array in main
        List<string> kelloxArgs = new();
        try
        {
            ArgumentPreProcessor.GroupArgs(options, ref specifiedFile, kelloxArgs, args);
        }
        catch (ArgumentError argumentError)
        {
            Console.WriteLine(argumentError.Message);
            Environment.Exit(64);
        }
        ArgumentPostProcessor.Process(options, specifiedFile);
    }

    /// <summary>
    /// Executes a Kellox program from a file
    /// </summary>
    internal static void RunFile(string path, bool onlyAnalyze)
    {
        byte[] file = File.ReadAllBytes(path);
        Run(Encoding.UTF8.GetString(file), onlyAnalyze);
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
    internal static void RunPrompt(bool onlyAnalyze)
    {
        PrintUtils.PrintPrompt();
        string? line = Console.ReadLine();
        // If the user hasn't given any input the console will close
        while (line is not "" && line is not null)
        {
            Run(line, onlyAnalyze);
            ErrorOccurred = false;
            // Adds linebreak if the writing position in the console is not at the left border
            if (Console.GetCursorPosition().Left is not 0)
            {
                Console.Write(Environment.NewLine);
            }
            PrintUtils.PrintPrompt();
            line = Console.ReadLine();
        }
    }

    /// <summary>
    /// Executes a Kellox program
    /// </summary>
    /// <param name="sourceCode">The sourcecode of the program that shall be executed</param>
    public static void Run(string sourceCode, bool onlyAnalyze)
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
            if (program.Runnable)
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
