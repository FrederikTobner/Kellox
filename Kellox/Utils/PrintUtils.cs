namespace Kellox.Utils
{
    internal static class PrintUtils
    {
        internal static void PrintKelloxHelp()
        {
            Console.WriteLine("usage: kellox [-h|-v||[-c file| file]]?");
            Console.WriteLine("These are common Kellox Interpreter commands:\nRun from Prompt\nkellox\nRun a Kellox file\nkellox file.klx\nAnaylize file\nkellox -a file.klx");
        }

        internal static void PrintKelloxVersion()
        {
            Console.WriteLine("Kellox 0.1");
        }

        internal static void PrintPrompt()
        {
            Console.Write(">>> ");
        }
    }
}
