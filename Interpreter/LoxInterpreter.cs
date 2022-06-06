using Interpreter.Exceptions;
using Interpreter.Expressions;
using Interpreter.Functions;
using Interpreter.Statements;
using Interpreter.Utils;

namespace Interpreter
{
    internal static class LoxInterpreter
    {
        /// <summary>
        /// The current Environment where the program is getting executed
        /// </summary>
        internal readonly static LoxEnvironment globalEnvironment = InitializeGlobal();

        /// <summary>
        /// Contains the local (not global expression) that reference a variabale from an outer scope
        /// </summary>
        internal static readonly Dictionary<IExpression, int> locals = new();

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
        /// Defines the native functions of lox
        /// </summary>
        private static void DefineNativeFunctions(LoxEnvironment loxEnvironment)
        {
            loxEnvironment.Define("clock", new ClockFunction());
            loxEnvironment.Define("wait", new WaitFunction());
            loxEnvironment.Define("clear", new ClearFunction());
            loxEnvironment.Define("read", new ReadFunction());
        }

        /// <summary>
        /// The current Environment where the program is getting executed
        /// </summary>
        internal static LoxEnvironment currentEnvironment = globalEnvironment;

        /// <summary>
        /// Interprets and executes a LoxProgram
        /// </summary>
        /// <param name="program">The program that shall be executed</param>
        internal static void Interpret(LoxProgram program)
        {
            Resolver.Resolve(program);
            if (!program.Runnable)
            {
                return;
            }
            RunProgram(program);
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
            LoxRunner.RunTimeErrorOccurred = true;
            LoxErrorLogger.Error(runTimeError.Token.Line, runTimeError.Message);
        }

        /// <summary>
        /// Adds the result of the re
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="depth"></param>
        internal static void Resolve(IExpression expression, int depth)
        {
            locals.Add(expression, depth);
        }
    }
}
