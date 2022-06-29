using Kellox.Interpreter;

namespace Kellox.Statements;

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
        Statements = statements;
    }

    /// <summary>
    /// Executes the statements in the block
    /// </summary>
    public void Execute()
    {
        // Saves the current environment
        KelloxEnvironment environment = KelloxInterpreter.currentEnvironment;
        // Creates a new Environment after execution
        KelloxInterpreter.currentEnvironment = new KelloxEnvironment(environment);
        //Executes all the statements in the Block
        foreach (IStatement statement in Statements)
        {
            statement.Execute();
        }
        // Resets the currentEnvironment
        KelloxInterpreter.currentEnvironment = environment;

    }
}
