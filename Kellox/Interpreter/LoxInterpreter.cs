using Lox.Expressions;
using Lox.Functions;
using Lox.i18n;
using Lox.Interpreter.Exceptions;
using Lox.Lexer;
using Lox.Parser;
using Lox.Resolver;
using Lox.Statements;
using Lox.Tokens;
using Lox.Utils;
using System.Text;

namespace Lox.Interpreter;

/// <summary>
/// Contains method to interpret a LoxProgram that was produced by the 
/// </summary>
internal static class LoxInterpreter
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
    internal readonly static LoxEnvironment globalEnvironment = InitializeGlobal();

    /// <summary>
    /// Initializes the global Environment
    /// </summary>
    private static LoxEnvironment InitializeGlobal()
    {
        LoxEnvironment globalEnvironment = new();
        DefineNativeFunctions(globalEnvironment);
        return globalEnvironment;
    }


    /// <summary>
    /// Contains the local (not global expression) that reference a variabale from an outer scope
    /// </summary>
    private static readonly Dictionary<IExpression, int> locals = new();


    /// <summary>
    /// The current Environment where the program is getting executed
    /// </summary>
    internal static LoxEnvironment currentEnvironment = globalEnvironment;

    /// <summary>
    /// Starts the interpreter (from file or as command prompt if no file is specified)
    /// </summary>
    /// <param name="args">The arguments provided by the user when the interpreter was started</param>
    internal static void RunLoxInterpreter(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine(Messages.toMuchArgsErrorMessage);
            //Exit code 64 -> The command was used incorrectly, wrong number of anguments
            Environment.Exit(64);
        }
        else if (args.Length is 1)
        {
            RunFile(args[0]);
        }
        else
        {
            RunPrompt();
        }
    }

    /// <summary>
    /// Executes a lox program from a file
    /// </summary>
    internal static void RunFile(string path)
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
            // The input data was incorrect -> Error during the lexical analysis or the parsing process
            Environment.Exit(65);
        }

    }

    /// <summary>
    /// Executes a lox program from the Command prompt
    /// </summary>
    internal static void RunPrompt()
    {
        Console.WriteLine(Constants.loxPromptMessage);
        string? line = Console.ReadLine();
        while (line is not "" && line is not null)
        {
            Run(line);
            ErrorOccurred = false;
            Console.WriteLine(Constants.loxPromptMessage);
            line = Console.ReadLine();
        }
    }

    /// <summary>
    /// Executes a lox program
    /// </summary>
    /// <param name="sourceCode">The sourcecode of the program that shall be executed</param>
    private static void Run(string sourceCode)
    {
        IReadOnlyList<Token> tokens = KelloxLexer.ScanTokens(sourceCode);
        LoxProgram program = KelloxParser.Parse(tokens);
        if (ErrorOccurred)
        {
            return;
        }
        if (program.Runnable)
        {
            Interpret(program);
        }
    }

    /// <summary>
    /// Interprets and executes a LoxProgram
    /// </summary>
    /// <param name="program">The program that shall be executed</param>
    internal static void Interpret(LoxProgram program)
    {
        KelloxResolver.Resolve(program);
        if (program.Runnable)
        {
            RunProgram(program);
        }
    }

    public static bool TryGetDepthOfLocal(IExpression expression, out int distance) => locals.TryGetValue(expression, out distance);

    /// <summary>
    /// Defines the native functions of lox
    /// </summary>
    private static void DefineNativeFunctions(LoxEnvironment loxEnvironment)
    {
        loxEnvironment.Define(Constants.ClockFunction, new ClockFunction());
        loxEnvironment.Define(Constants.WaitFunction, new WaitFunction());
        loxEnvironment.Define(Constants.ClearFunction, new ClearFunction());
        loxEnvironment.Define(Constants.ReadFunction, new ReadFunction());
        loxEnvironment.Define(Constants.RandomFunction, new RandomFunction());
    }



    /// <summary>
    /// Executes the Program written in Lox after the variables in the inner scopes have been resolved
    /// </summary>
    /// <param name="program">The program that is executed</param>
    private static void RunProgram(LoxProgram program)
    {
        foreach (IStatement statement in program)
        {
            try
            {
                statement.ExecuteStatement();
            }
            catch (RunTimeError runTimeError)
            {
                ReportRunTimeError(runTimeError);
                break;
            }
        }
    }

    /// <summary>
    /// Reports a error that has orrured during runtime
    /// </summary>
    /// <param name="runTimeError">The Error that has occured during runtime</param>
    private static void ReportRunTimeError(RunTimeError runTimeError)
    {
        RunTimeErrorOccurred = true;
        ErrorLogger.Error(runTimeError.Token.Line, runTimeError.Message);
    }

    /// <summary>
    /// Stores the distance beetween the scopes, that local (not gloabl) variables are referenced and declared
    /// </summary>
    /// <param name="expression">The Expression referencing a Variable</param>
    /// <param name="distance">The distance between the scopes</param>
    internal static void Resolve(IExpression expression, int distance)
    {
        locals.Add(expression, distance);
    }
}
