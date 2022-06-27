using Kellox.Arguments;
using Kellox.Exceptions;
using System.Text.RegularExpressions;

namespace Kellox.Interpreter.Arguments;

internal static class ArgumentPreProcessor
{
    /// <summary>
    /// Regex for a Kellox file
    /// </summary>
    private const string KelloxFilePattern = @".*\.klx$";

    /// <summary>
    /// Regex for an option
    /// </summary>
    private const string OptionPattern = @"-[a-zA-Z]|--[a-zA-Z]+$";

    /// <summary>
    /// Groups the Arguments together based on their type
    /// </summary>
    /// <param name="options">The options of that were used</param>
    /// <param name="specifiedFile">The specified file -> null if no file is specified</param>
    /// <param name="kelloxArgs"></param>
    /// <param name="args">The additional arguments passed in for the Kelloxprogram</param>
    /// <exception cref="ArgumentError"></exception>
    internal static void GroupArgs(List<string> options, ref string? specifiedFile, List<string> kelloxArgs, params string[] args)
    {
        //Type of the last argument that was processed
        ArgumentType lastArgumentType = ArgumentType.None;
        foreach (string? argument in args)
        {
            if (argument is not null)
            {
                if (Regex.IsMatch(argument, OptionPattern))
                {
                    if (lastArgumentType is ArgumentType.KELLOXARGUMENT or ArgumentType.FILE)
                    {
                        throw new ArgumentError("Options must be at the beginning");
                    }
                    options.Add(argument);
                    lastArgumentType = ArgumentType.OPTION;
                }
                else if (Regex.IsMatch(argument, KelloxFilePattern))
                {
                    if (lastArgumentType is ArgumentType.KELLOXARGUMENT)
                    {
                        throw new ArgumentError("File has to follow after options");
                    }
                    else if (lastArgumentType is ArgumentType.FILE)
                    {
                        throw new ArgumentError("Can not interpret multiple files");
                    }
                    specifiedFile = argument;
                    lastArgumentType = ArgumentType.FILE;
                }
                else
                {
                    kelloxArgs.Add(argument);
                    lastArgumentType = ArgumentType.KELLOXARGUMENT;
                }
            }
        }
    }
}
