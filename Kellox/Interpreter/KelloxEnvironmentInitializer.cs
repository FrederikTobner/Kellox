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
    private static void DefineNativeFunctions(KelloxEnvironment loxEnvironment)
    {
        loxEnvironment.Define(BeepFunctionName, new BeepFunction());
        loxEnvironment.Define(ClearFunctionName, new ClearFunction());
        loxEnvironment.Define(ClockFunctionName, new ClockFunction());
        loxEnvironment.Define(ExitFunctionName, new ExitFunction());
        loxEnvironment.Define(RandomFunctionName, new RandomFunction());
        loxEnvironment.Define(ReadFunctionName, new ReadFunction());
        loxEnvironment.Define(TypeOfFunctionName, new TypeOfFunction());
        loxEnvironment.Define(WaitFunctionName, new WaitFunction());
    }
}
