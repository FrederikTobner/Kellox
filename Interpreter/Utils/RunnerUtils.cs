using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Utils
{
    public static class RunnerUtils
    {
        private const string emptyString = "";


        /// <summary>
        /// Boolean value indicating wheather an error has ocurred during the interpretation of the program
        /// </summary>
        public static bool ErrorOccurred { get; internal set; }

        /// <summary>
        /// Starts the interpreter (from file or as command prompt if no file is specified)
        /// </summary>
        /// <param name="args"></param>
        internal static void RunInterpreter(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("To much args");
                Environment.Exit(64);
            }
            else if (args.Length is 1)
            {
                RunnerUtils.RunFile(args[0]);
            }
            else
            {
                RunnerUtils.RunPrompt();
            }
        }

        /// <summary>
        /// Executes a lox program from a file
        /// </summary>
        internal static void RunFile(string path)
        {
            byte[] file = File.ReadAllBytes(path);
            Run(Encoding.UTF8.GetString(file));
            if (ErrorOccurred)
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
            while (line is not emptyString && line is not null)
            {
                Run(line);
                ErrorOccurred = false;
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
            LoxProgram program = new(parser.Parse());
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
