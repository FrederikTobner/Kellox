﻿using Kellox.Statements;
using System.Collections;

namespace Kellox.Interpreter;

/// <summary>
/// Model of a program written in Kellox
/// </summary>
public class KelloxProgram : IEnumerable<IStatement>
{
    /// <summary>
    /// A readonly list containing of parsed statements/instructions that can be executed
    /// </summary>
    public IReadOnlyList<IStatement> Statements { get; init; }

    /// <summary>
    /// Boolean value that determines whether the program is runnable
    /// </summary>
    public bool Runnable => Statements is not null && !KelloxInterpreter.ErrorOccurred;

    /// <summary>
    /// The constructor of the KelloxProgram class
    /// </summary>
    /// <param name="statements">The statements that make up the KelloxProgram</param>
    public KelloxProgram(IReadOnlyList<IStatement> statements)
    {
        Statements = statements;
    }

    /// <summary>
    /// Enumeration over the specific KelloxProgram
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
