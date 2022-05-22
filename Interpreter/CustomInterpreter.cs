using Interpreter.Exceptions;
using Interpreter.Stmt;

namespace Interpreter
{
    internal class CustomInterpreter
    {
        internal static void Interpret(List<IStatement> statements)
        {
            try
            {
                foreach (var statement in statements)
                {
                    statement.ExecuteStatement();
                }
            }
            catch (RunTimeError ex)
            {
                ReportRunTimeError(ex);
            }
        }

        private static void ReportRunTimeError(RunTimeError runTimeError)
        {
            Program.Error(runTimeError.Token.Line, runTimeError.Message);
        }
    }
}
