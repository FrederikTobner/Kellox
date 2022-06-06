﻿using Interpreter.Exceptions;

namespace Interpreter
{
    /// <summary>
    /// Environment for the Programming Language
    /// Associates values to variables
    /// </summary>
    internal class LoxEnvironment
    {
        /// <summary>
        /// The enclosing Environment 
        /// e.g. 'global' if the Scope is definied in the global Scope
        /// </summary>
        private readonly LoxEnvironment? enclosing;

        /// <summary>
        /// Dictionary that conntains all the values defined in this Environment
        /// </summary>
        private readonly Dictionary<string, object?> values;

        public LoxEnvironment(LoxEnvironment? environment = null)
        {
            values = new Dictionary<string, object?>();
            this.enclosing = environment;
        }

        /// <summary>
        /// Defines a new variable
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value that shall be assigned to the variable</param>
        public void Define(string name, object? value)
        {
            values.Add(name, value);
        }

        /// <summary>
        /// Gets the value associated to a specific Variable
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns>The value associated to the variable</returns>
        /// <exception cref="RunTimeError">If the Variable is undefiened</exception>
        public object? Get(Token name)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }
            if (enclosing is not null)
            {
                return enclosing.Get(name);
            }
            throw new RunTimeError(name, "Undefiened variable \'" + name.Lexeme + "\'.");
        }

        public object? GetAt(int distance, Token token)
        {
            return Ancestor(distance).Get(token);
        }

        private LoxEnvironment Ancestor(int depth)
        {
            LoxEnvironment environment = this;
            for (int i = 0; i < depth; i++)
            {
                if (environment.enclosing is not null)
                {
                    environment = environment.enclosing;
                }
            }
            return environment;
        }

        /// <summary>
        /// Assigns a value to a variable in the environment
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value that shall be assigned to the variable</param>
        /// <exception cref="RunTimeError">If the Variable is undefiened</exception>
        public void Assign(Token name, object? value)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                values[name.Lexeme] = value;
                return;
            }
            if (enclosing is not null)
            {
                enclosing.Assign(name, value);
                return;
            }
            throw new RunTimeError(name, "Variable not defined yet. Assignment impossible");
        }

        public void AssignAt(int depth, Token name, object? value)
        {
            Ancestor(depth).values[name.Lexeme] = value;
        }
    }
}
