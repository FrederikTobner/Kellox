using Interpreter.Exceptions;
using Interpreter.Expressions;
using Interpreter.Statements;
using System.Text;

namespace Interpreter
{
    internal static class CustomInterpreter
    {
        /// <summary>
        /// Boolean value indicating wheather an error has ocurred during the interpretation of the program
        /// </summary>
        private static bool errorOccurred;

        internal static CustomEnvironment currentEnvironment = new();

        /// <summary>
        /// Interprets and executes a List of Statements/Program
        /// </summary>
        /// <param name="progeam">The program that shall be executed</param>
        internal static void Interpret(CustomProgram program)
        {
            try
            {
                foreach (IStatement statement in program)
                {
                    statement.ExecuteInnerStatements();
                }
            }
            catch (RunTimeError ex)
            {
                ReportRunTimeError(ex);
            }
        }

        /// <summary>
        /// Reports a error that has orrured during runtime
        /// </summary>
        /// <param name="runTimeError">The Error that has occured during runtime</param>
        private static void ReportRunTimeError(RunTimeError runTimeError)
        {
            Error(runTimeError.Token.Line, runTimeError.Message);
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
                RunFile(args[0]);
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

        /// <summary>
        /// Executes from a file
        /// </summary>
        private static void RunFile(string path)
        {
            byte[] file = File.ReadAllBytes(path);
            Run(Encoding.UTF8.GetString(file));
            if (errorOccurred)
            {
                Environment.Exit(65);
            }
        }

        /// <summary>
        /// Executes from a Command prompt
        /// </summary>
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

        /// <summary>
        /// Executes a lux program
        /// </summary>
        /// <param name="programCode">The program that shall be executed</param>
        private static void Run(string programCode)
        {
            TokenScanner scanner = new(programCode);
            List<Token> tokens = scanner.ScanTokens();
            Parser parser = new(tokens);
            CustomProgram program = new(parser.Parse());
            if (errorOccurred)
            {
                return;
            }
            if (program.Runnable)
            {
                Interpret(program);
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
    }
}
