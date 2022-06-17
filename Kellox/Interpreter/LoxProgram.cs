using Lox.Statements;
using Lox.Utils;
using System.Collections;

namespace Lox.Interpreter;

/// <summary>
/// Model of a program written in Lox
/// </summary>
internal class LoxProgram : IEnumerable<IStatement>
{
    /// <summary>
    /// A readonly list conatining the parsed statements
    /// </summary>
    public IReadOnlyList<IStatement> Statements { get; init; }

    /// <summary>
    /// Boolean value that determines whether the program is runnable
    /// </summary>
    public bool Runnable => Statements is not null && !LoxInterpreter.ErrorOccurred;

    /// <summary>
    /// The constructor of the LoxProgram class
    /// </summary>
    /// <param name="statements">The statements the LoxProgram contains</param>
    public LoxProgram(IReadOnlyList<IStatement> statements)
    {
        Statements = statements;
    }

    /// <summary>
    /// Enumeration over the specific LoxProgram
    /// </summary>
    public IEnumerator<IStatement> GetEnumerator()
    {
        for (int i = 0; i < Statements.Count; i++)
        {
            yield return Statements[i];
        }
    }

    /// <summary>
    /// Exposes Enumerator
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
