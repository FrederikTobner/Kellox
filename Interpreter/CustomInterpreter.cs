using Interpreter.Exceptions;

namespace Interpreter
{
    internal class CustomInterpreter
    {
        internal static void Interpret(Expression expression)
        {
            try
            {
                object? value = expression.EvaluateExpression();
                Console.WriteLine(value?.ToString());
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
