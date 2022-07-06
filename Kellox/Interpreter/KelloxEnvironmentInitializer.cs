using Kellox.Functions;

namespace Kellox.Interpreter;

internal static class KelloxEnvironmentInitializer
{
    private const string BeepFunctionName = "beep";
    private const string ClearFunctionName = "clear";
    private const string ClockFunctionName = "clock";
    private const string ExitFunctionName = "exit";
    private const string RandomFunctionName = "random";
    private const string ReadFunctionName = "read";
    private const string TypeOfFunctionName = "typeof";
    private const string WaitFunctionName = "wait";

    /// <summary>
    /// Initializes the global Environment
    /// </summary>
    internal static KelloxEnvironment InitializeGlobal()
    {
        KelloxEnvironment globalEnvironment = new();
        DefineNativeFunctions(globalEnvironment);
        return globalEnvironment;
    }

    /// <summary>
    /// Defines the native functions of lox
    /// </summary>
    private static void DefineNativeFunctions(KelloxEnvironment kelloxEnvironment)
    {
        kelloxEnvironment.Define(BeepFunctionName, new BeepFunction());
        kelloxEnvironment.Define(ClearFunctionName, new ClearFunction());
        kelloxEnvironment.Define(ClockFunctionName, new ClockFunction());
        kelloxEnvironment.Define(ExitFunctionName, new ExitFunction());
        kelloxEnvironment.Define(RandomFunctionName, new RandomFunction());
        kelloxEnvironment.Define(ReadFunctionName, new ReadFunction());
        kelloxEnvironment.Define(TypeOfFunctionName, new TypeOfFunction());
        kelloxEnvironment.Define(WaitFunctionName, new WaitFunction());
    }
}
