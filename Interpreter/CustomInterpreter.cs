using Interpreter.Exceptions;
using Interpreter.Statements;

namespace Interpreter
{
    internal static class CustomInterpreter
    {
        /// <summary>
        /// The current Environment where the program is getting executed
        /// </summary>
        internal static CustomEnvironment currentEnvironment = new();

        /// <summary>
        /// Interprets and executes a List of Statements/Program
        /// </summary>
        /// <param name="program">The program that shall be executed</param>
        internal static void Interpret(CustomProgram program)
        {
            try
            {
                foreach (IStatement statement in program)
                {
                    statement.ExecuteStatements();
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
