namespace Interpreter;
class Program
{
    private const string sampleProgramPath = ".\\SampleProgram.txt";

    static void Main(string[] args)
    {
        //CustomInterpreter.TestExpression();
        //CustomInterpreter.TestInterpreter(args);
        CustomInterpreter.TestInterpreterFromFile(sampleProgramPath);
    }
}
