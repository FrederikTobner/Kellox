using Interpreter.Statements;
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
        /// Boolean value that determines weather the program is runnable
        /// </summary>
        public bool Runnable => Statements is not null;

        public LoxProgram(IReadOnlyList<IStatement> statements)
        {
            this.Statements = statements;
        }

        /// <summary>
        /// Expoes the Enumerator for CustomProgram
        /// </summary>
        public IEnumerator<IStatement> GetEnumerator()
        {
            for (int i = 0; i < Statements.Count; i++)
            {
                yield return Statements[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
