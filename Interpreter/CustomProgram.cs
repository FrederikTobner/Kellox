using Interpreter.Statements;
using System.Collections;

namespace Interpreter
{
    /// <summary>
    /// Model of a program written in Lox
    /// </summary>
    internal class CustomProgram : IEnumerable<IStatement>
    {
        private readonly List<IStatement> statements;

        /// <summary>
        /// Boolean value that determines weather the program is runnable
        /// </summary>
        public bool Runnable => statements is not null;

        public CustomProgram(List<IStatement> statements)
        {
            this.statements = statements;
        }

        /// <summary>
        /// Expoes the Enumerator for CustomProgram
        /// </summary>
        public IEnumerator<IStatement> GetEnumerator()
        {
            for (int i = 0; i < statements.Count; i++)
            {
                yield return statements[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
