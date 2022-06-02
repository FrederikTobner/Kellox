﻿using Interpreter.Exceptions;
using Interpreter.Statements;

namespace Interpreter.Functions
{
    internal class LoxFunction : ILoxCallable
    {
        /// <summary>
        /// The function statement where the function is defined
        /// </summary>
        private readonly FunctionStatement declaration;

        public LoxFunction(FunctionStatement declaration)
        {
            this.declaration = declaration;
        }

        public int Arity => this.declaration.Parameters.Count;

        public object? Call(List<object?> arguments)
        {
            LoxEnvironment environment = new(LoxInterpreter.globalEnvironment);
            for (int i = 0; i < declaration.Parameters.Count; i++)
            {
                environment.Define(declaration.Parameters[i].Lexeme, arguments[i]);
            }
            LoxEnvironment oldEnvironment = LoxInterpreter.currentEnvironment;
            LoxInterpreter.currentEnvironment = environment;
            try
            {
                new BlockStatement(declaration.Body).ExecuteStatement();
            }
            catch (Return returnValue)
            {
                return returnValue.Value;
            }
            LoxInterpreter.currentEnvironment = oldEnvironment;
            return null;
        }

        public override string ToString() => "<fn " + declaration.Name.Lexeme + ">";
    }
}
