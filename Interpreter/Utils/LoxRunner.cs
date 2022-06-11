using System.Text;

namespace Interpreter.Utils
{
    public static class LoxRunner
    {
        private const string emptyString = "";
        private const string loxPrompt = "> ";
        private const string toMuchArgsErrorMessage = "To much args";

        /// <summary>
        /// Boolean value indicating wheather an error has ocurred during the interpretation of the program
        /// </summary>
        public static bool ErrorOccurred { get; internal set; }

        /// <summary>
        /// Boolean value indicating wheather an error has ocurred while running the program
        /// </summary>
        public static bool RunTimeErrorOccurred { get; internal set; }

        /// <summary>
        /// Starts the interpreter (from file or as command prompt if no file is specified)
        /// </summary>
        /// <param name="args"></param>
        internal static void RunInterpreter(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine(toMuchArgsErrorMessage);
                //Exit code 64 -> The command was used incorrectly, wrong number of anguments
                Environment.Exit(64);
            }
            else if (args.Length is 1)
            {
                LoxRunner.RunFile(args[0]);
            }
            else
            {
                LoxRunner.RunPrompt();
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
            Console.WriteLine(loxPrompt);
            string? line = Console.ReadLine();
            while (line is not emptyString && line is not null)
            {
                Run(line);
                ErrorOccurred = false;
                Console.WriteLine(loxPrompt);
                line = Console.ReadLine();
            }
        }

        /// <summary>
        /// Executes a lox program
        /// </summary>
        /// <param name="sourceCode">The sourcecode of the program that shall be executed</param>
        private static void Run(string sourceCode)
        {
            Lexer lexer = new(sourceCode);
            List<Token> tokens = lexer.ScanTokens();
            Parser parser = new(tokens);
            LoxProgram program = parser.Parse();
            if (ErrorOccurred)
            {
                return;
            }
            if (program.Runnable)
            {
                LoxInterpreter.Interpret(program);
            }
        }
    }
}
