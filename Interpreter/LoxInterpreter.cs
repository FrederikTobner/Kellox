using Interpreter.Exceptions;
using Interpreter.Statements;

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
            try
            {
                foreach (IStatement statement in program)
                {
                    statement.ExecuteStatement();
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
            Program.Error(runTimeError.Token.Line, runTimeError.Message);
        }
    }
}
