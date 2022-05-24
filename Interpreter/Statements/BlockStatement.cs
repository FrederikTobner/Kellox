﻿using Interpreter.Statements;

namespace Interpreter
{
    internal class BlockStatement : IStatement
    {
        private readonly List<IStatement> statements;

        public BlockStatement(List<IStatement> statements)
        {
            this.statements = statements;
        }

        /// <summary>
        /// Executes the statements in the block
        /// </summary>
        public void ExecuteStatements()
        {
            CustomEnvironment environment = CustomInterpreter.currentEnvironment;
            CustomInterpreter.currentEnvironment = new CustomEnvironment(environment);
            foreach (IStatement statement in statements)
            {
                statement.ExecuteStatements();
            }
            CustomInterpreter.currentEnvironment = environment;
        }
    }
}