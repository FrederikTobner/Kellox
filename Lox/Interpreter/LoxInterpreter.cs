using Lox.Expressions;
using Lox.Functions;
using Lox.Interpreter.Exceptions;
using Lox.Resolver;
using Lox.Statements;
using Lox.Utils;

namespace Lox.Interpreter;

/// <summary>
/// Contains method to interpret a LoxProgram that was produced by the 
/// </summary>
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
        LoxResolver.Resolve(program);
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
    /// Stores the distance beetween the scopes, that local (not gloabl) variables are referenced and declared
    /// </summary>
    /// <param name="expression">The Expression referencing a Variable</param>
    /// <param name="distance">The distance between the scopes</param>
    internal static void Resolve(IExpression expression, int distance)
    {
        locals.Add(expression, distance);
    }
}
