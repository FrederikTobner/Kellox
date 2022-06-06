using Interpreter.Statements;

namespace Interpreter
{
    /// <summary>
    /// Models a block statement
    /// </summary>
    internal class BlockStatement : IStatement
    {
        /// <summary>
        /// The statements inside this block statement
        /// </summary>
        public IReadOnlyList<IStatement> Statements { get; init; }

        /// <summary>
        /// Constructor of the BLockStatement
        /// </summary>
        /// <param name="statements">The List of Statements in this BlockStatement</param>
        public BlockStatement(List<IStatement> statements)
        {
            this.Statements = statements;
        }

        /// <summary>
        /// Executes the statements in the block
        /// </summary>
        public void ExecuteStatement()
        {
            // Saves the current environment
            LoxEnvironment environment = LoxInterpreter.currentEnvironment;

            // Creates a new Environment after execution
            LoxInterpreter.currentEnvironment = new LoxEnvironment(environment);

            foreach (IStatement statement in Statements)
            {
                statement.ExecuteStatement();
            }

            // Resets the currentEnvironment
            LoxInterpreter.currentEnvironment = environment;

        }
    }
}
