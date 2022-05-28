using Interpreter.Expressions;
using System.Text;

namespace Interpreter;
class Program
{
    /// <summary>
    /// Boolean value indicating wheather an error has ocurred during the interpretation of the program
    /// </summary>
    private static bool errorOccurred;

    /// <summary>
    /// Path of the SampleProgram
    /// </summary>
    private static readonly string sampleProgramPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\SampleProgram.txt";

    static void Main(string[] args)
    {
        //TestExpression();
        //TestInterpreter(args);
        TestInterpreterFromFile(sampleProgramPath);
    }

    /// <summary>
    /// Executes a lox program from a file
    /// </summary>
    internal static void RunFile(string path)
    {
        byte[] file = File.ReadAllBytes(path);
        Run(Encoding.UTF8.GetString(file));
        if (Program.errorOccurred)
        {
            Environment.Exit(65);
        }
    }

    /// <summary>
    /// Executes a lox program from the Command prompt
    /// </summary>
    internal static void RunPrompt()
    {
        Console.WriteLine("> ");
        string? line = Console.ReadLine();
        while (line != string.Empty && line is not null)
        {
            Run(line);
            Program.errorOccurred = false;
            Console.WriteLine("> ");
            line = Console.ReadLine();
        }
    }

    /// <summary>
    /// Executes a lox program
    /// </summary>
    /// <param name="sourceCode">The sourcecode of the program that shall be executed</param>
    private static void Run(string sourceCode)
    {
        Lexer scanner = new(sourceCode);
        List<Token> tokens = scanner.ScanTokens();
        Parser parser = new(tokens);
        CustomProgram program = new(parser.Parse());
        if (errorOccurred)
        {
            return;
        }
        if (program.Runnable)
        {
            CustomInterpreter.Interpret(program);
        }
    }

    /// <summary>
    /// Reports an error that occured during the lexical analysis
    /// </summary>
    /// <param name="line">The line where the error has occured</param>
    /// <param name="where"></param>
    /// <param name="message"></param>
    private static void Report(int line, string where, string message)
    {
        Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
        errorOccurred = true;
    }

    /// <summary>
    /// Reports an error that occured during the lexical analysis in specific line
    /// </summary>
    internal static void Error(int line, string message) => Report(line, "", message);

    /// <summary>
    /// Reports an error that occured during the lexical analysis that was caused by a specific token
    /// </summary>
    internal static void Error(Token token, string message)
    {
        switch (token.TokenType)
        {
            case TokenType.EOF:
                Report(token.Line, " at end", message);
                break;
            default:
                Report(token.Line, " at '" + token.Lexeme + "'", message);
                break;
        }
    }


    /// <summary>
    /// Tests the interpreter (from file or as command prompt if no file is specified)
    /// </summary>
    /// <param name="args"></param>
    internal static void TestInterpreter(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("To much args");
            Environment.Exit(64);
        }
        else if (args.Length is 1)
        {
            Program.RunFile(args[0]);
        }
        else
        {
            RunPrompt();
        }
    }

    /// <summary>
    /// Tests the interrpreter by forcing it to read the specified file
    /// </summary>
    /// <param name="path">The path of the file</param>
    internal static void TestInterpreterFromFile(string path)
    {
        RunFile(path);
    }

    /// <summary>
    /// Tests printing of a nested Expression
    /// </summary>
    internal static void TestExpression()
    {
        IExpression expression = new BinaryExpression(
        new UnaryExpression(
            new Token(TokenType.MINUS, "-", null, 1),
            new LiteralExpression(123)),
        new Token(TokenType.STAR, "*", null, 1),
        new GroupingExpression(
            new LiteralExpression(45.67)));

        Console.WriteLine(expression.ToString());
    }

}
