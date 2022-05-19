using System.Text;

namespace Interpreter;
class CustomInterpreter
{
    static Boolean errorOccurred = false;
    static void Main(string[] args)
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
        // For now, just prints the tokens.
        foreach (Token? token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
        errorOccurred = true;
    }

    internal static void Error(int line, string message) => Report(line, "", message);
}
