using Interpreter.Statements;
using Interpreter.Utils;
using System.Collections;

namespace Interpreter
{
    /// <summary>
    /// Model of a program written in Lox
    /// </summary>
    internal class LoxProgram : IEnumerable<IStatement>
    {
        public IReadOnlyList<IStatement> Statements { get; init; }

        /// <summary>
        /// Boolean value that determines whether the program is runnable
        /// </summary>
        public bool Runnable => Statements is not null && !LoxRunner.ErrorOccurred;

        /// <summary>
        /// The constructor of the LoxProgram class
        /// </summary>
        /// <param name="statements">The statements the LoxProgram contains</param>
        public LoxProgram(IReadOnlyList<IStatement> statements)
        {
            this.Statements = statements;
        }

        /// <summary>
        /// Implementation of the Enumeration over the specific LoxProgram
        /// </summary>
        public IEnumerator<IStatement> GetEnumerator()
        {
            for (int i = 0; i < Statements.Count; i++)
            {
                yield return Statements[i];
            }
        }

        /// <summary>
        /// Expoes the Enumerator for CustomProgram
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
