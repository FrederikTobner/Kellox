using Kellox.Interpreter;
using Kellox.Utils;

namespace Kellox.Arguments;

internal static class ArgumentPostProcessor
{
    internal static void Process(List<string> options, string? specifiedFile)
    {
        bool onlyAnalyze = false;
        foreach (string? option in options)
        {
            switch (option)
            {
                case "-h" or "--help":
                    PrintUtils.PrintKelloxHelp();
                    Environment.Exit(0);
                    break;
                case "-v" or "--version":
                    PrintUtils.PrintKelloxVersion();
                    Environment.Exit(0);
                    break;
                case "-a":
                    if (specifiedFile is null)
                    {
                        Console.WriteLine("file not specified, but option \'-a\' used");
                        PrintUtils.PrintKelloxHelp();
                        //Exit code 64 -> The command was used incorrectly, unsupported options
                        Environment.Exit(64);
                    }
                    onlyAnalyze = true;
                    break;
                default:
                    Console.WriteLine("unknown option: " + option);
                    PrintUtils.PrintKelloxHelp();
                    //Exit code 64 -> The command was used incorrectly, unsupported options
                    Environment.Exit(64);
                    break;
            }
        }

        if (specifiedFile is null)
        {
            PrintUtils.PrintKelloxVersion();
            KelloxInterpreter.RunPrompt(onlyAnalyze);
        }

        if (specifiedFile is not null)
        {
            KelloxInterpreter.RunFile(specifiedFile, onlyAnalyze);
        }
    }
}

