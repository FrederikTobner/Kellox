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

        public BlockStatement(List<IStatement> statements)
        {
            this.Statements = statements;
        }

        /// <summary>
        /// Executes the statements in the block
        /// </summary>
        public void ExecuteStatements()
        {
            CustomEnvironment environment = CustomInterpreter.currentEnvironment;
            CustomInterpreter.currentEnvironment = new CustomEnvironment(environment);
            foreach (IStatement statement in Statements)
            {
                statement.ExecuteStatements();
            }
            CustomInterpreter.currentEnvironment = environment;
        }
    }
}
