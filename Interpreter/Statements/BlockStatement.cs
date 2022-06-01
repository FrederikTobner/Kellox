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
        public void ExecuteStatement()
        {
            LoxEnvironment environment = LoxInterpreter.currentEnvironment;
            LoxInterpreter.currentEnvironment = new LoxEnvironment(environment);
            foreach (IStatement statement in Statements)
            {
                statement.ExecuteStatement();
            }
            LoxInterpreter.currentEnvironment = environment;
        }
    }
}
