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

        private static LoxEnvironment InitializeGlobal()
        {
            LoxEnvironment loxEnvironment = new();
            loxEnvironment.Define("clock", new ClockFunction());
            loxEnvironment.Define("wait", new WaitFunction());
            loxEnvironment.Define("clear", new ClearFunction());
            loxEnvironment.Define("read", new ReadFunction());
            return loxEnvironment;
        }

        /// <summary>
        /// The current Environment where the program is getting executed
        /// </summary>
        internal static LoxEnvironment currentEnvironment = globalEnvironment;

        /// <summary>
        /// Interprets and executes a List of Statements/Program
        /// </summary>
        /// <param name="program">The program that shall be executed</param>
        internal static void Interpret(LoxProgram program)
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
                }
            }
        }

        /// <summary>
        /// Reports a error that has orrured during runtime
        /// </summary>
        /// <param name="runTimeError">The Error that has occured during runtime</param>
        private static void ReportRunTimeError(RunTimeError runTimeError)
        {
            RunnerUtils.RunTimeErrorOccurred = true;
            ErrorUtils.Error(runTimeError.Token.Line, runTimeError.Message);
        }

        internal static void Resolve(IExpression expression, int depth)
        {
            //locals.Add(expression, depth);
        }
    }
}
