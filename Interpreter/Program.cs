using Interpreter.Expr;
using System.Text;

namespace Interpreter;
class Program
{
    static bool errorOccurred = false;
    static void Main(string[] args)
    {
        //TestExpression();
        TestInterpreter(args);

    }

    private static void TestInterpreter(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("To much args");
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

    private static void TestExpression()
    {
        Expression expression = new BinaryExpression(
        new UnaryExpression(
            new Token(TokenType.MINUS, "-", null, 1),
            new LiteralExpression(123)),
        new Token(TokenType.STAR, "*", null, 1),
        new GroupingExpression(
            new LiteralExpression(45.67)));

        Console.WriteLine(expression.ToString());
    }
    private static void RunFile(string path)
    {
        byte[] file = File.ReadAllBytes(path);
        Run(Encoding.UTF8.GetString(file));
        if (errorOccurred)
        {
            Environment.Exit(65);
        }
    }

    private static void RunPrompt()
    {
        Console.WriteLine("> ");
        string? line = Console.ReadLine();
        while (line != string.Empty && line is not null)
        {
            Run(line);
            errorOccurred = false;
            Console.WriteLine("> ");
            line = Console.ReadLine();
        }
    }

    private static void Run(string file)
    {
        TokenScanner scanner = new(file);
        List<Token> tokens = scanner.ScanTokens();
        Parser parser = new(tokens);
        Expression? expression = parser.Parse();
        if (errorOccurred)
        {
            return;
        }
        if (expression is not null)
        {
            CustomInterpreter.Interpret(expression);
        }
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
        errorOccurred = true;
    }

    internal static void Error(int line, string message) => Report(line, "", message);

    internal static void Error(Token token, string message)
    {
        if (token.TokenType == TokenType.EOF)
        {
            Report(token.Line, " at end", message);
        }
        else
        {
            Report(token.Line, " at '" + token.Lexeme + "'", message);
        }
    }
}
