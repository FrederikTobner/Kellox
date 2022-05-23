using Interpreter.Statements;

namespace Interpreter
{
    internal class BlockStatement : IStatement
    {
        List<IStatement> statements;

        public BlockStatement(List<IStatement> statements)
        {
            this.statements = statements;
        }

        public void ExecuteStatement()
        {
            CustomEnvironment environment = CustomInterpreter.customEnvironment;
            CustomInterpreter.customEnvironment = new CustomEnvironment(environment);
            foreach (IStatement statement in statements)
            {
                statement.ExecuteStatement();
            }
            CustomInterpreter.customEnvironment = environment;
        }
    }
}
