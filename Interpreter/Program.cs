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
        for (; ; )
        {
            Console.WriteLine("> ");
            string? line = Console.ReadLine();
            if (line is null)
            {
                break;
            }
            Run(line);
            errorOccurred = false;
        }
    }

    private static void Run(string file)
    {
        Scanner scanner = new(file);
        List<Token> tokens = scanner.ScanTokens();

        // For now, just print the tokens.
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

    internal static void Error(int line, string message)
    {
        Report(line, "", message);
    }
}
